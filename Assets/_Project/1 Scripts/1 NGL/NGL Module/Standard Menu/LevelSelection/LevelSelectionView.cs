using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using ArcadianLab.SimFramework.Data;


namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public class LevelSelectionView : View
    {
        public static event Action PlayLevel;
        [SerializeField] private TextMeshProUGUI _textChapterName;
        [SerializeField] private Button _buttonPlay;

        public override void Init()
        {
            base.Init();
            Assert.IsNotNull(_textChapterName);
            Assert.IsNotNull(_buttonPlay);
            _buttonPlay.onClick.AddListener(() => PlayLevel?.Invoke());
        }

        public override ViewState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                OnInternalStateUpdated();
            }
        }

        private void OnEnable()
        {
            LevelSelection.StateChanged += OnParentStateUpdated;
            /*SignUp.StateChanged += OnParentStateUpdated;
            Validation.ValidationFailed += ShowErrorMessage;
            SignUp.SignUpFailed += ShowErrorMessage;*/
        }

        private void OnDisable()
        {
            LevelSelection.StateChanged -= OnParentStateUpdated;
            /*SignUp.StateChanged -= OnParentStateUpdated;
            Validation.ValidationFailed -= ShowErrorMessage;
            SignUp.SignUpFailed -= ShowErrorMessage;*/
        }

        protected override void OnParentStateUpdated(Enum type)
        {
            LevelSelectionState levelSelectionState = (LevelSelectionState)type;
            if (State == ViewState.None)
            {
                if (levelSelectionState == LevelSelectionState._Init) State = ViewState._Init;
            }
            else if (State == ViewState._Init || State == ViewState._Dispose)
            {
                if (levelSelectionState == LevelSelectionState.LevelSelection) State = ViewState.ShowDefaultView;
            }
            else if (State == ViewState.ShowDefaultView)
            {
                if (levelSelectionState == LevelSelectionState.ChapterSelection || levelSelectionState == LevelSelectionState._Dispose)
                    State = ViewState._Dispose;
            }
            else
            {
                if (levelSelectionState == LevelSelectionState._Dispose) State = ViewState._Dispose;
            }
        }

        protected override void OnInternalStateUpdated()
        {
            base.OnInternalStateUpdated();
            if (State == ViewState.ShowDefaultView) UpdateView();
        }

        private void UpdateView()
        {
            Chapter chapter = DataManager.Instance.data.gameData.userSelectedChapter;
            _textChapterName.text = $"Chapter {chapter.id + 1} [{ chapter.name}]";
        }
    }
}
