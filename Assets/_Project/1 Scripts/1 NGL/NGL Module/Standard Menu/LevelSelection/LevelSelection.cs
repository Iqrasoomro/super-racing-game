using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.NGL.Utils;

namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public enum LevelSelectionState
    {
        None,
        _Init,
        HudHomeView, // +MainMenuView
        ChapterSelection,
        LevelSelection,
        _Dispose,
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Managers)]
    public class LevelSelection : SubManagerWithHomeView
    {
        [SerializeField] private LevelSelectionState _state;
        public static event Action<Enum> StateChanged;
        public static event Action<Level> SelectedLevel;
        public static event Action<Level> RequestLevelComplete;
        public static event Action ChapterComplete;
        //...

        private Chapter userSelectedChapter;
        private List<LevelThumb> levelThumbs;
        private LevelThumb userSelectedThumb;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform levelThumbs_ParentPanel;
        [SerializeField] private LevelThumb levelThumbPrefab;
        private RectTransform levelThumbs_ParentPanel_RectTransform;
        private int widthPerPrefabAddition;

        public override void Init()
        {
            base.Init();
            Assert.IsNotNull(scrollRect);
            Assert.IsNotNull(levelThumbs_ParentPanel);
            Assert.IsNotNull(levelThumbPrefab);
            userSelectedChapter = DataManager.Instance.data.gameData.userSelectedChapter;
            levelThumbs = new List<LevelThumb>();
            userSelectedThumb = null;
            levelThumbs_ParentPanel_RectTransform = levelThumbs_ParentPanel.GetComponent<RectTransform>();

            DynamicLayoutCalculations();
            InstantiatePopulateLevels_ForUserSelectedChapter();
        }

        private void OnEnable()
        {
            StandardMenu.StateChanged += OnParentManager_StateUpdated;
            ChapterSelection.SelectedChapter += delegate (Chapter chapter ){ userSelectedChapter = chapter; };
            Chapter.AfterLevelCompleted += OnUnlockNextLevel;
            LevelThumb.OnLevelThumbClicked += OnLevelThumbClicked;
            GameData.AfterChapterComplete += OnChapterCompleted;
        }

        private void OnDisable()
        {
            StandardMenu.StateChanged -= OnParentManager_StateUpdated;
            ChapterSelection.SelectedChapter += delegate (Chapter chapter) { userSelectedChapter = chapter; };
            Chapter.AfterLevelCompleted -= OnUnlockNextLevel;
            LevelThumb.OnLevelThumbClicked -= OnLevelThumbClicked;
            GameData.AfterChapterComplete -= OnChapterCompleted;
        }

        protected override void OnParentManager_StateUpdated(Enum type)
        {
            StandardMenuState standardMenuState = (StandardMenuState)type;
            if (State == LevelSelectionState.None)
            {
                if (standardMenuState == StandardMenuState._Init) State = LevelSelectionState._Init;
            }
            else if (State == LevelSelectionState._Init || State == LevelSelectionState._Dispose)
            {
                if (standardMenuState == StandardMenuState.LevelSelection) State = LevelSelectionState.LevelSelection;
            }
            else if (State == LevelSelectionState.LevelSelection)
            {
                if (standardMenuState == StandardMenuState.HudHomeView ||
                    standardMenuState == StandardMenuState.ChapterSelection ||
                    standardMenuState == StandardMenuState._Dispose ) State = LevelSelectionState._Dispose;
            }
            else
            {
                if (standardMenuState == StandardMenuState._Dispose ) State = LevelSelectionState._Dispose;
            }
        }

        public LevelSelectionState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                StateChanged?.Invoke(_state);
                OnInternalStateUpdated();
            }
        }

        protected override void OnInternalStateUpdated()
        {
            Debug.Log($"{gameObject.name} OnInternalStateUpdated :{_state}");
            if (State == LevelSelectionState._Init) Init();
            else if (State == LevelSelectionState.LevelSelection) PopulateLevels_ForUserSelectedChapter();
        }

        void DynamicLayoutCalculations()
        {
            HorizontalLayoutGroup levelThumbs_ParentPanel_HorizontalGroup =
                levelThumbs_ParentPanel.GetComponent<HorizontalLayoutGroup>();
            Assert.IsNotNull(levelThumbs_ParentPanel_HorizontalGroup);
            int spacing = (int)levelThumbs_ParentPanel_HorizontalGroup.spacing;

            LayoutElement levelThumbPrefab_LayoutElement = levelThumbPrefab.GetComponent<LayoutElement>();
            Assert.IsNotNull(levelThumbPrefab_LayoutElement);
            widthPerPrefabAddition = (int)levelThumbPrefab_LayoutElement.minWidth + spacing;
            levelThumbs_ParentPanel_RectTransform.sizeDelta = new Vector2(spacing, levelThumbs_ParentPanel_RectTransform.sizeDelta.y);
        }

        void InstantiatePopulateLevels_ForUserSelectedChapter()
        {
            if (levelThumbs.Count > 0) return; // Prefabs already instantiated, wrong call
            foreach (Level level in userSelectedChapter.levels)
            {
                LevelThumb levelThumb = Instantiate(levelThumbPrefab, levelThumbs_ParentPanel);
                levelThumb.Init(level);
                levelThumbs.Add(levelThumb);
                if (level.state == LevelState.Unlocked_Current) userSelectedThumb = levelThumb;
            }
            if (!userSelectedThumb) SelectThumb(levelThumbs[0]); //SelectThumb_UserSelectedLevel_AllLevelsPlayed();
            ExpandScrollContentPanel_ForLevelThumbs();
            ScrollToCurrentLevel();
        }

        void PopulateLevels_ForUserSelectedChapter()
        {
            if (levelThumbs.Count <= 0) throw new Exception("PopulateLevelsForSelectedChapter() failed, thumbs not instantiated");

            /*if (this.userSelectedChapter.id == userSelectedChapter.id) return;
            else this.userSelectedChapter = userSelectedChapter;*/

            userSelectedThumb = null;
            foreach (Level level in userSelectedChapter.levels)
            {
                levelThumbs[level.id].level = level;
                levelThumbs[level.id].UpdateThumbPrefab(level.state);
                if (level.state == LevelState.Unlocked_Current) userSelectedThumb = levelThumbs[level.id];
            }

            if (!userSelectedThumb) SelectThumb(levelThumbs[userSelectedChapter.userSelectedLevel.id]); //SelectThumb_UserSelectedLevel_AllLevelsPlayed();
            ScrollToCurrentLevel();
        }

        private void ExpandScrollContentPanel_ForLevelThumbs() 
        {
            levelThumbs_ParentPanel_RectTransform.sizeDelta =
                    new Vector2(levelThumbs_ParentPanel_RectTransform.sizeDelta.x + 
                    widthPerPrefabAddition * DataManager.Instance.data.gameData.userSelectedChapter.levels.Count,
                    levelThumbs_ParentPanel_RectTransform.sizeDelta.y);
        }

        void OnLevelThumbClicked(LevelThumb _levelThumb)
        {
            if (_levelThumb == userSelectedThumb) return;
            if (_levelThumb.State == LevelState.Unlocked_Played || _levelThumb.State == LevelState.Unlocked_Current)
            {
                userSelectedThumb = _levelThumb;
                SelectedLevel?.Invoke(userSelectedThumb.level);
            }
            else
            {
                if (_levelThumb.State == LevelState.Locked_Soft || _levelThumb.State == LevelState.Locked_Hard)
                    PopupMessage.Instance.Show(Constants.Data.levelLockedMessage[_levelThumb.level.state]);
            }
        }

        void OnUnlockNextLevel(Level currentLevel, Level nextLevel)
        {
            if(currentLevel != null)
            {
                levelThumbs[currentLevel.id - 1].UpdateThumbPrefab(LevelState.Unlocked_Played);
                levelThumbs[currentLevel.id].UpdateThumbPrefab(LevelState.Unlocked_Current);
                userSelectedThumb = levelThumbs[currentLevel.id];
                ScrollToCurrentLevel();

                if (nextLevel != null) levelThumbs[nextLevel.id].UpdateThumbPrefab(LevelState.Locked_Soft);
            }
            else
            {
                // No (new) current level for this chapter, all levels complete, unlock next chapter
                ChapterComplete?.Invoke();
                userSelectedThumb.EnableSelectionFrame(true);
            }
        }

        void OnChapterCompleted(Chapter currentChapter, Chapter nextChapter)
        {
            if (currentChapter == null) return;
            userSelectedThumb = null;
            int index = 0;
            List<Level> levels = currentChapter.levels;
            foreach (LevelThumb levelThumb in levelThumbs)
            {
                levelThumb.level = levels[index++];
                levelThumb.UpdateThumbPrefab(levelThumb.level.state);
                if (levelThumb.level.state == LevelState.Unlocked_Current) userSelectedThumb = levelThumb;
            }
            SelectThumb(levelThumbs[userSelectedChapter.userSelectedLevel.id]);
        }

        void SelectThumb(LevelThumb _userSelectedThumb)
        {
            userSelectedThumb = _userSelectedThumb;
            if (_userSelectedThumb) userSelectedThumb.EnableSelectionFrame(true);
        }

        void ScrollToCurrentLevel()
        {
            if (!userSelectedThumb) return;
            float scrollValue = (((float)(userSelectedThumb.level.id + 1)) /
            ((float)(DataManager.Instance.data.gameData.userSelectedChapter.levels.Count)));
            Utils.ScrollRect_.ScrollToPercent(scrollRect, scrollValue);
        }

        /*private void OnGUI()
        {
            GUI.skin.label.fontSize = 20;
            GUI.skin.button.fontSize = 20;
            if (userSelectedThumb) GUILayout.Label($"UserSelectedThumb = {userSelectedThumb.level.id+1}");
            if (GUILayout.Button("Complete level")) RequestLevelComplete?.Invoke(userSelectedThumb.level);
            if (GUILayout.Button("Complete Chapter")) RequestLevelComplete?.Invoke(userSelectedThumb.level);
        }*/
    }
}
