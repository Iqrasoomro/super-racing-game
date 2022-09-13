using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;
using ArcadianLab.SimFramework.Data;

namespace ArcadianLab.SimFramework.Settings
{
    public class SettingsCoreView : View
    {
        public static event Action<bool> EnableMusic;
        public static event Action<bool> EnableSound;
        [SerializeField] private Button _buttonClose;
        [SerializeField] private Toggle _toggleBackgroundMusic;
        [SerializeField] private Toggle _toggleSoundEffects;

        public override void Init()
        {
            base.Init();

            Assert.IsNotNull(_buttonClose);
            Assert.IsNotNull(_toggleBackgroundMusic);
            Assert.IsNotNull(_toggleSoundEffects);

            _buttonClose.onClick.AddListener(() => State = ViewState.HideDefaultView);
            // Fire events from here to on/off music/sounds to AudioManager, also receive value on start
            _toggleBackgroundMusic.onValueChanged.AddListener(delegate { EnableMusic?.Invoke(_toggleBackgroundMusic.isOn); });
            _toggleSoundEffects.onValueChanged.AddListener(delegate { EnableSound?.Invoke(_toggleSoundEffects.isOn); });
            _toggleBackgroundMusic.SetIsOnWithoutNotify(DataManager.Instance.data.audioData.isMusicOn);
            _toggleSoundEffects.SetIsOnWithoutNotify(DataManager.Instance.data.audioData.isSoundOn);
        }

        private void OnEnable() => SettingsManager.StateChanged += OnParentStateUpdated;
        private void OnDisable() => SettingsManager.StateChanged -= OnParentStateUpdated;

        protected override void OnParentStateUpdated(Enum type)
        {
            SettingsState settingsState = (SettingsState)type;
            if (State == ViewState.None && settingsState == SettingsState._Init) State = ViewState._Init;
            else
            {
                if (settingsState == SettingsState.SettingsCore) State = ViewState.ShowDefaultView;
                else if (settingsState == SettingsState.Hide) State = ViewState.HideDefaultView;
                else if (settingsState == SettingsState._Dispose) State = ViewState._Dispose;
            }
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
    }
}