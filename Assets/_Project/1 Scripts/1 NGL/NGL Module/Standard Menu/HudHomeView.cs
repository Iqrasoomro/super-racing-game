using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public class HudHomeView : View
    {
        public static event Action OnBackButtonClicked;
        public static event Action OnSettingsButtonClicked;
        [SerializeField] private Button _buttonBack;
        [SerializeField] private Button _buttonSettings;

        public override void Init()
        {
            base.Init();
            Assert.IsNotNull(_buttonBack);
            Assert.IsNotNull(_buttonSettings);
            _buttonBack.onClick.AddListener(() => OnBackButtonClicked?.Invoke());
            _buttonSettings.onClick.AddListener(() => OnSettingsButtonClicked?.Invoke());
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
            StandardMenu.StateChanged += ToggleBackButton;
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
            StandardMenuState standardMenuState = (StandardMenuState) type;
            if (State == ViewState.None)
            {
                if (standardMenuState == StandardMenuState._Init) State = ViewState._Init;
            }
            else if (State == ViewState._Init || State == ViewState._Dispose)
            {
                if (standardMenuState == StandardMenuState.HudHomeView ||
                    standardMenuState == StandardMenuState.ChapterSelection ||
                    standardMenuState == StandardMenuState.LevelSelection) State = ViewState.ShowDefaultView;
            }
            else
            {
                if (standardMenuState == StandardMenuState._Dispose) State = ViewState._Dispose;
            }
        }

        private void ToggleBackButton(Enum state) => _buttonBack.gameObject.SetActive(!((StandardMenuState)state == StandardMenuState.HudHomeView));
    }
}
