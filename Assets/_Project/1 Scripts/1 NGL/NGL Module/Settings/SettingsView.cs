using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.NGL.StdMenu;

namespace ArcadianLab.SimFramework.NGL.Settings
{
    public class SettingsView : View
    {
        public static event Action<bool> EnableMusic;
        public static event Action<bool> EnableSound;
        [SerializeField] private Button _buttonClose;


        public override void Init()
        {
            base.Init();

            Assert.IsNotNull(_buttonClose);

            _buttonClose.onClick.AddListener(() => State = ViewState.HideDefaultView);
            // Fire events from here to on/off music/sounds to AudioManager, also receive value on start
            /*_toggleBackgroundMusic.onValueChanged.AddListener(delegate { EnableMusic?.Invoke(_toggleBackgroundMusic.isOn); });
            _toggleSoundEffects.onValueChanged.AddListener(delegate { EnableSound?.Invoke(_toggleSoundEffects.isOn); });
            _toggleBackgroundMusic.SetIsOnWithoutNotify(DataManager.Instance.data.audioData.isMusicOn);
            _toggleSoundEffects.SetIsOnWithoutNotify(DataManager.Instance.data.audioData.isSoundOn);*/
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
            HudHomeView.OnSettingsButtonClicked += delegate { State = ViewState.ShowDefaultView; };
            /*SignUp.StateChanged += OnParentStateUpdated;
            Validation.ValidationFailed += ShowErrorMessage;
            SignUp.SignUpFailed += ShowErrorMessage;*/
        }

        private void OnDisable()
        {
            StandardMenu.StateChanged -= OnParentStateUpdated;
            HudHomeView.OnSettingsButtonClicked -= delegate { State = ViewState.ShowDefaultView; };
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
            /*else if (State == ViewState._Init || State == ViewState.HideDefaultView || State == ViewState._Dispose)
            {
                if (standardMenuState == StandardMenuState.Settings) State = ViewState.ShowDefaultView;
            }
            else if (State == ViewState.ShowDefaultView)
            {
                if (standardMenuState != StandardMenuState.Settings)
                    State = ViewState._Dispose;
            }*/
            else
            {
                if (standardMenuState == StandardMenuState._Dispose) State = ViewState._Dispose;
            }
        }
    }
}