using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.Utils;

namespace ArcadianLab.SimFramework.Settings
{
    public class SettingsHomeView : View
    {
        public static event Action ShowGallery;
        [SerializeField] private Button _buttonClose;
        [SerializeField] private Button _buttonGallery;
        [SerializeField] private Button _buttonInviteFriends;
        [SerializeField] private Button _buttonSettings;
        [SerializeField] private Button _buttonAccount;
        [SerializeField] private Button _buttonHelpAbout;

        public override void Init()
        {
            base.Init();

            Assert.IsNotNull(_buttonClose);
            Assert.IsNotNull(_buttonGallery);
            Assert.IsNotNull(_buttonInviteFriends);
            Assert.IsNotNull(_buttonSettings);
            Assert.IsNotNull(_buttonAccount);
            Assert.IsNotNull(_buttonHelpAbout);

            _buttonClose.onClick.AddListener(() => SettingsManager.Instance.State = SettingsState.Hide);
            _buttonGallery.onClick.AddListener(() => ShowGallery?.Invoke());
            _buttonInviteFriends.onClick.AddListener(() => InviteFriends());
            _buttonSettings.onClick.AddListener(() => SettingsManager.Instance.State = SettingsState.SettingsCore);
            _buttonAccount.onClick.AddListener(() => SettingsManager.Instance.State = SettingsState.Account);
            _buttonHelpAbout.onClick.AddListener(() => OpenUrl.OpenHomepage());
        }

        private void OnEnable() => SettingsManager.StateChanged += OnParentStateUpdated;
        private void OnDisable() => SettingsManager.StateChanged -= OnParentStateUpdated;

        protected override void OnParentStateUpdated(Enum type)
        {
            SettingsState settingsState = (SettingsState)type;
            if (State == ViewState.None && settingsState == SettingsState._Init) State = ViewState._Init;
            else
            {
                if (settingsState == SettingsState.SidePanelHome) State = ViewState.ShowDefaultView;
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

        private void InviteFriends() { } // => new NativeShare().SetUrl(Constants.ShareLink).Share();
    }
}