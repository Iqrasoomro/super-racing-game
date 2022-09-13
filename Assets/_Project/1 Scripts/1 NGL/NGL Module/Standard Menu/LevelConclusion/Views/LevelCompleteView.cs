using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public class LevelCompleteView : View
    {
        public static event Action NextLevel;
        [SerializeField] private Button _buttonNext;

        public override void Init()
        {
            base.Init();
            Assert.IsNotNull(_buttonNext);
            _buttonNext.onClick.AddListener(() => NextLevel?.Invoke());
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

        private void OnEnable() => LevelConclusion.StateChanged += OnParentStateUpdated;

        private void OnDisable() => LevelConclusion.StateChanged -= OnParentStateUpdated;

        protected override void OnParentStateUpdated(Enum type)
        {
            LevelConclusionState levelConclusionState = (LevelConclusionState)type;
            if (State == ViewState.None)
            {
                if (levelConclusionState == LevelConclusionState._Init) State = ViewState._Init;
            }
            else if (State == ViewState._Init || State == ViewState._Dispose)
            {
                if (levelConclusionState == LevelConclusionState.LevelComplete) State = ViewState.ShowDefaultView;
            }
            else
            {
                if (levelConclusionState == LevelConclusionState._Dispose) State = ViewState._Dispose;
            }
        }
    }
}
