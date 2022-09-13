using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public class MainMenuView : View
    {
        public static event Action OnPlayButtonClicked;
        [SerializeField] private Button _buttonPlay;

        public override void Init()
        {
            base.Init();
            Assert.IsNotNull(_buttonPlay);
            _buttonPlay.onClick.AddListener(() => OnPlayButtonClicked?.Invoke());
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
            StandardMenu.StateChanged += OnParentStateUpdated;
            /*SignUp.StateChanged += OnParentStateUpdated;
            Validation.ValidationFailed += ShowErrorMessage;
            SignUp.SignUpFailed += ShowErrorMessage;*/
        }
        private void OnDisable()
        {
            StandardMenu.StateChanged -= OnParentStateUpdated;
            /*SignUp.StateChanged -= OnParentStateUpdated;
            Validation.ValidationFailed -= ShowErrorMessage;
            SignUp.SignUpFailed -= ShowErrorMessage;*/
        }

        protected override void OnParentStateUpdated(Enum type)
        {
            StandardMenuState standardMenuState = (StandardMenuState)type;
            if (State == ViewState.None)
            {
                if (standardMenuState == StandardMenuState._Init) State = ViewState._Init;
            }
            else if (State == ViewState._Init || State == ViewState._Dispose)
            {
                if (standardMenuState == StandardMenuState.HudHomeView) State = ViewState.ShowDefaultView;
            }
            else if (State == ViewState.ShowDefaultView)
            {
                if (standardMenuState == StandardMenuState.ChapterSelection || standardMenuState == StandardMenuState._Dispose)
                    State = ViewState._Dispose;
            }
            else
            {
                if (standardMenuState == StandardMenuState._Dispose) State = ViewState._Dispose;
            }
        }
    }
}
