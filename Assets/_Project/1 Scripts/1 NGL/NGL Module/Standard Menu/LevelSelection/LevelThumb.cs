using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using ArcadianLab.SimFramework.Data;

namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    [Serializable]
    public class LevelState_GameObject
    {
        public LevelState state;
        public GameObject gameObj; 
    }


    [Serializable]
    [RequireComponent(typeof(LayoutElement))] // Dependency, when width for scoll panel is dynamically calc.
    public class LevelThumb : Object_
    {
        public static event Action<LevelThumb> OnLevelThumbClicked;
        public Level level;
        public LevelState State { get; private set; }
        [SerializeField] private TextMeshProUGUI number;
        [SerializeField] private List<LevelState_GameObject> levelStatesObjects;
        [SerializeField] private GameObject selectionFrame;
        private Button button;
        private static bool isSequenceValidatedOnetime = false;


        public override void Init() { } // Null overriding, using cutom Init() w/ params

        public void Init(Level level)
        {
            button = GetComponent<Button>();
            Assert.IsNotNull(button);
            Assert.IsNotNull(number);
            Assert.IsNotNull(name);
            Assert.IsNotNull(levelStatesObjects);
            Assert.IsNotNull(selectionFrame);
            Assert.AreEqual(Enum.GetNames(typeof(LevelState)).Length - 1, levelStatesObjects.Count);
            if (!isSequenceValidatedOnetime) ValidateChapterThumb_InternalStatesSequence();

            this.level = level;
            number.text = (this.level.id + 1).ToString();
            State = this.level.state;
            button.interactable = true;
            button.onClick.AddListener(() => OnLevelThumb_ButtonClicked());
            selectionFrame.gameObject.SetActive(false);
            UpdateThumbPrefab(State);
        }

        private void OnEnable() => LevelSelection.SelectedLevel += OnLevelSelected;
        private void OnDisable() => LevelSelection.SelectedLevel -= OnLevelSelected;

        private void ValidateChapterThumb_InternalStatesSequence()
        {
            isSequenceValidatedOnetime = true; // Lock for other level thumbs, one time validation is enough
            for (int i = 0; i < levelStatesObjects.Count; i++)
            {
                if (levelStatesObjects[i].state != (LevelState)(i)) 
                    throw new Exception("LevelState enum sequence is not followed in ChapterThumb.chapterStatesObjects in inspector");
            }
        }

        public void UpdateThumbPrefab(LevelState state)
        {
            level.state = State = state;
            foreach (LevelState_GameObject levelStatesObject in levelStatesObjects)
            {
                bool result = levelStatesObject.state == this.State;
                levelStatesObject.gameObj.SetActive(result);
            }
            EnableSelectionFrame(state == LevelState.Unlocked_Current);
        }

        private void OnLevelThumb_ButtonClicked()
        {
            Debug.Log((level.id+1) + ": Button pressed");
            OnLevelThumbClicked?.Invoke(this);
        }

        private void OnLevelSelected(Level userSelectedLevel) => EnableSelectionFrame(level.id == userSelectedLevel.id);

        public void EnableSelectionFrame(bool value) => selectionFrame.gameObject.SetActive(value);
    }
}
