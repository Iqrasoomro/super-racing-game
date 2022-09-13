using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using ArcadianLab.SimFramework.Core;

namespace ArcadianLab.SimFramework.Utils
{
    public class PopupMessage : Object_
    {
        public static PopupMessage Instance;
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _textMessage;
        [SerializeField] private Button _buttonClose;

        public override void Init()
        {
            Assert.IsNotNull(_panel);
            Assert.IsNotNull(_textMessage);
            Assert.IsNotNull(_buttonClose);
            Instance = this;
            _buttonClose.onClick.AddListener(() => Show(false));
            Show(false);

        }

        private void Start() => Show(false);

        public void Show(bool value, string message = null)
        {
            _textMessage.text = message;
            if (_panel) _panel.gameObject.SetActive(value && message != null);
        }
    }
}
