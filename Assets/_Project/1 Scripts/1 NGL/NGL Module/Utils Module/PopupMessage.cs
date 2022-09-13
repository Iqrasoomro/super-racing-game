using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using System.Collections;

namespace ArcadianLab.SimFramework.NGL.Utils
{
    public class PopupMessage : Object_
    {
        public static PopupMessage Instance;
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _textMessage;
        [SerializeField] private Button _buttonClose;
        private Action callbackAction_OnClose;

        public override void Init()
        {
            Assert.IsNotNull(_panel);
            Assert.IsNotNull(_textMessage);
            Assert.IsNotNull(_buttonClose);
            Instance = this;
            _buttonClose.onClick.AddListener(() => Close());
            Show(false);
        }

        private void Start() => Show(false);

        private void Show(bool value) => _panel.gameObject.SetActive(value);

        private void Close()
        {
            Show(false);
            callbackAction_OnClose?.Invoke();
            callbackAction_OnClose = null;
        }

        public void Show(string message = null)
        {
            if (message == null) return;
            _textMessage.text = message;
            Show(true);
        }

        public void Show(string message = null, float delay = 1f, Action action = null)
        {
            if (message == null) return;
            if (delay <= 0 || delay > 5f) delay = 1f;
            _textMessage.text = message;
            if (action != null) callbackAction_OnClose = action;
            StartCoroutine(DelayedShow(message, delay));
        }

        private IEnumerator DelayedShow(string message, float delay)
        {
            yield return new WaitForSeconds(delay);
            Show(message);
        }
    }
}
