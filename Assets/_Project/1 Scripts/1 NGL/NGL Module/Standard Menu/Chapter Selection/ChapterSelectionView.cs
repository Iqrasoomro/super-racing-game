using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public class ChapterSelectionView : View
    {
        public static event Action OnNextButtonClicked;
        [SerializeField] private Button _buttonNext;

        public override void Init()
        {
            base.Init();
            Assert.IsNotNull(_buttonNext);
            _buttonNext.onClick.AddListener(() => OnNextButtonClicked?.Invoke());
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
            ChapterSelection.StateChanged += OnParentStateUpdated;
            /*SignUp.StateChanged += OnParentStateUpdated;
            Validation.ValidationFailed += ShowErrorMessage;
            SignUp.SignUpFailed += ShowErrorMessage;*/
        }

        private void OnDisable()
        {
            ChapterSelection.StateChanged -= OnParentStateUpdated;
            /*SignUp.StateChanged -= OnParentStateUpdated;
            Validation.ValidationFailed -= ShowErrorMessage;
            SignUp.SignUpFailed -= ShowErrorMessage;*/
        }

        protected override void OnParentStateUpdated(Enum type)
        {
            ChapterSelectionState chapterSelectionState = (ChapterSelectionState)type;
            if (State == ViewState.None)
            {
                if (chapterSelectionState == ChapterSelectionState._Init) State = ViewState._Init;
            }
            else if (State == ViewState._Init || State == ViewState._Dispose)
            {
                if (chapterSelectionState == ChapterSelectionState.ChapterSelection) State = ViewState.ShowDefaultView;
            }
            else if (State == ViewState.ShowDefaultView)
            {
                if (chapterSelectionState == ChapterSelectionState.HudHomeView || 
                    chapterSelectionState == ChapterSelectionState.LevelSelection ||
                    chapterSelectionState == ChapterSelectionState._Dispose)
                    State = ViewState._Dispose;
            }
            else
            {
                if (chapterSelectionState == ChapterSelectionState._Dispose) State = ViewState._Dispose;
            }
        }
    }
}
