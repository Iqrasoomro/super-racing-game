using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.NGL.Utils;


namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public enum ChapterSelectionState
    {
        None,
        _Init,
        HudHomeView, // +MainMenuView
        ChapterSelection,
        LevelSelection,
        _Dispose,
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Managers)]
    public class ChapterSelection : SubManagerWithHomeView
    {
        [SerializeField] private ChapterSelectionState _state;
        public static event Action<Enum> StateChanged;
        public static event Action<Chapter> SelectedChapter;
        private List<ChapterThumb> chapterThumbs;
        [SerializeField] private ChapterThumb userSelectedThumb;
        [SerializeField] private Transform chapterThumbs_ParentPanel;
        [SerializeField] private ChapterThumb chapterThumbPrefab;

        public override void Init()
        {
            base.Init();
            Assert.IsNotNull(chapterThumbs_ParentPanel);
            Assert.IsNotNull(chapterThumbPrefab);
            chapterThumbs = new List<ChapterThumb>();
            userSelectedThumb = null;
            InstantiateChapterPrefabs();
            DataManager.Instance.data.gameData.SetCurrentChapter_AsUserSelectedChapter();
        }

        private void OnEnable()
        {
            StandardMenu.StateChanged += OnParentManager_StateUpdated;
            GameData.AfterChapterComplete += OnAfterChapterComplete;
            ChapterThumb.OnChapterThumbClicked += OnChapterThumbClicked;
        }

        private void OnDisable()
        {
            StandardMenu.StateChanged -= OnParentManager_StateUpdated;
            GameData.AfterChapterComplete -= OnAfterChapterComplete;
            ChapterThumb.OnChapterThumbClicked -= OnChapterThumbClicked;
        }

        protected override void OnParentManager_StateUpdated(Enum type)
        {
            StandardMenuState standardMenuState = (StandardMenuState)type;
            if (State == ChapterSelectionState.None)
            {
                if (standardMenuState == StandardMenuState._Init) State = ChapterSelectionState._Init;
            }
            else if (State == ChapterSelectionState._Init || State == ChapterSelectionState._Dispose)
            {
                if (standardMenuState == StandardMenuState.ChapterSelection) State = ChapterSelectionState.ChapterSelection;
            }
            else if (State == ChapterSelectionState.ChapterSelection)
            {
                if (standardMenuState == StandardMenuState.HudHomeView ||
                    standardMenuState == StandardMenuState.LevelSelection ||
                    standardMenuState == StandardMenuState._Dispose ) State = ChapterSelectionState._Dispose;
            }
            else
            {
                if (standardMenuState == StandardMenuState._Dispose ) State = ChapterSelectionState._Dispose;
            }
        }

        public ChapterSelectionState State
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
            if (State == ChapterSelectionState._Init) Init();
        }

        void InstantiateChapterPrefabs()
        {
            if (chapterThumbs.Count > 0) return; // Prefabs already instantiated, wrong call
            foreach (Chapter chapter in DataManager.Instance.data.gameData.chapters)
            {
                ChapterThumb chapterThumb = Instantiate(chapterThumbPrefab, chapterThumbs_ParentPanel);
                chapterThumb.Init(chapter);
                chapterThumbs.Add(chapterThumb);
                if (chapter.state == ChapterState.Unlocked_Current) userSelectedThumb = chapterThumb;
            }
            if (!userSelectedThumb) SelectThumb_UserSelectedChapter_AllChaptersPlayed();
        }

        void OnChapterThumbClicked(ChapterThumb _chapterThumb)
        {
            if (_chapterThumb == userSelectedThumb) return;
            if (_chapterThumb.State == ChapterState.Unlocked_Played || 
                _chapterThumb.State == ChapterState.Unlocked_Current) SelectThumb(_chapterThumb);
            else
            {
                if (_chapterThumb.State == ChapterState.Locked_Hard || _chapterThumb.State == ChapterState.Locked_ComingSoon)
                    PopupMessage.Instance.Show(Constants.Data.chapterLockedMessage[_chapterThumb.chapter.state]);
            }
        }

        void OnAfterChapterComplete(Chapter currentChapter, Chapter nextChapter)
        {
            if (currentChapter != null)
            {
                chapterThumbs[currentChapter.id - 1].UpdateThumbPrefab(ChapterState.Unlocked_Played);
                chapterThumbs[currentChapter.id].UpdateThumbPrefab(currentChapter.state);
                SelectThumb(chapterThumbs[currentChapter.id]);
            }
        }

        void SelectThumb_UserSelectedChapter_AllChaptersPlayed()
        {
            if (DataManager.Instance.data.gameData.userSelectedChapter == null) return;
            foreach (ChapterThumb chapterThumb in chapterThumbs)
            {
                if (chapterThumb.chapter.id == DataManager.Instance.data.gameData.userSelectedChapter.id)
                {
                    SelectThumb(chapterThumb);
                    break;
                }
            }
        }

        void SelectThumb(ChapterThumb _userSelectedThumb)
        {
            userSelectedThumb = _userSelectedThumb;
            SelectedChapter?.Invoke(userSelectedThumb.chapter);
            if (_userSelectedThumb) userSelectedThumb.EnableSelectionFrame(true);
        }

        /*private void OnGUI()
        {
            GUI.skin.label.fontSize = 20;
            GUI.skin.button.fontSize = 20;
            if (userSelectedThumb) GUILayout.Label($"UserSelectedThumb = {userSelectedThumb.chapter.name}");
            if (GUILayout.Button("Complete chapter")) RequestChapterComplete?.Invoke(userSelectedThumb.chapter);
        }*/
    }
}
