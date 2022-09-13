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
    public class ChapterState_GameObject
    {
        public ChapterState state;
        public GameObject gameObj; 
    }


    [Serializable]
    public class ChapterThumb : Object_
    {
        public static event Action<ChapterThumb> OnChapterThumbClicked;
        public Chapter chapter;
        public ChapterState State { get; private set; }
        [SerializeField] private TextMeshProUGUI number;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private List<ChapterState_GameObject> chapterStatesObjects;
        [SerializeField] private GameObject selectionFrame;
        private Button button;
        private static bool isSequenceValidatedOnetime = false;


        public override void Init() { } // Null overriding, using cutom Init() w/ params

        public void Init(Chapter chapter)
        {
            button = GetComponent<Button>();
            Assert.IsNotNull(button);
            Assert.IsNotNull(number);
            Assert.IsNotNull(number);
            Assert.IsNotNull(name);
            Assert.IsNotNull(chapterStatesObjects);
            Assert.IsNotNull(selectionFrame);
            Assert.AreEqual(Enum.GetNames(typeof(ChapterState)).Length - 1, chapterStatesObjects.Count);
            if (!isSequenceValidatedOnetime) ValidateChapterThumb_InternalStatesSequence();

            this.chapter = chapter;
            number.text = (this.chapter.id+1).ToString();
            name.text = this.chapter.name;
            State = this.chapter.state;
            button.interactable = true;
            button.onClick.AddListener(() => OnChapterThumb_ButtonClicked());
            selectionFrame.gameObject.SetActive(false);
            UpdateThumbPrefab(State);
        }

        private void OnEnable() => ChapterSelection.SelectedChapter += OnChapterSelected;
        private void OnDisable() => ChapterSelection.SelectedChapter -= OnChapterSelected;

        /// <summary>
        /// ChapterState enum have an order that must be in accordance with chapter objects assigned 
        /// with their respective states in inspector
        /// </summary>
        private void ValidateChapterThumb_InternalStatesSequence()
        {
            isSequenceValidatedOnetime = true; // Lock for other chapter thumbs, one time validation is enough
            for (int i = 0; i < chapterStatesObjects.Count; i++)
            {
                if (chapterStatesObjects[i].state != (ChapterState)(i)) 
                    throw new Exception("ChapterState enum sequence is not followed in ChapterThumb.chapterStatesObjects in inspector");
            }
        }

        public void UpdateThumbPrefab(ChapterState state)
        {
            chapter.state = State = state;
            foreach (ChapterState_GameObject chapterStatesObject in chapterStatesObjects)
            {
                bool result = chapterStatesObject.state == this.State;
                chapterStatesObject.gameObj.SetActive(result);
            }
            EnableSelectionFrame(state == ChapterState.Unlocked_Current);
        }

        private void OnChapterThumb_ButtonClicked()
        {
            Debug.Log(chapter.name + ": Button pressed");
            OnChapterThumbClicked?.Invoke(this);
        }

        private void OnChapterSelected(Chapter userSelectedChapter) => EnableSelectionFrame(chapter.id == userSelectedChapter.id);

        public void EnableSelectionFrame(bool value) => selectionFrame.gameObject.SetActive(value);
    }
}
