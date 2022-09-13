using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Data;


namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public enum LevelConclusionState
    {
        None,
        _Init,
        HudHomeView, // +MainMenuView
        ChapterSelection,
        LevelSelection,
        LevelConclusion, // Home View
        LevelComplete,
        LevelFailed,
        AfterLevelConclusion,
        _Dispose,
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Managers)]
    public class LevelConclusion : SubManagerWithHomeView
    {
        [SerializeField] private LevelConclusionState _state;
        public static event Action<Enum> StateChanged;
        public static event Action AfterLevelConclusion_ViewsClosed;

        public override void Init()
        {
            
        }

        private void OnEnable()
        {
            StandardMenu.StateChanged += OnParentManager_StateUpdated;
            LevelCompleteView.NextLevel += delegate { State = LevelConclusionState.AfterLevelConclusion; };
            LevelFailedView.NextLevel += delegate { State = LevelConclusionState.AfterLevelConclusion; };
            //Chapter.LevelCompleted += OnLevelCompleted;
        }

        private void OnDisable()
        {
            StandardMenu.StateChanged -= OnParentManager_StateUpdated;
            LevelCompleteView.NextLevel -= delegate { State = LevelConclusionState.AfterLevelConclusion; };
            LevelFailedView.NextLevel += delegate { State = LevelConclusionState.AfterLevelConclusion; };
            //Chapter.LevelCompleted -= OnLevelCompleted;
        }

        protected override void OnParentManager_StateUpdated(Enum type)
        {
            StandardMenuState standardMenuState = (StandardMenuState)type;
            if (State == LevelConclusionState.None)
            {
                if (standardMenuState == StandardMenuState._Init) State = LevelConclusionState._Init;
            }
            else if (State == LevelConclusionState._Init || State == LevelConclusionState._Dispose)
            {
                if (standardMenuState == StandardMenuState.LevelConclusion) State = LevelConclusionState.LevelConclusion;
            }
            else if (State == LevelConclusionState.LevelSelection)
            {
                if (standardMenuState == StandardMenuState.HudHomeView ||
                    standardMenuState == StandardMenuState.ChapterSelection ||
                    standardMenuState == StandardMenuState.LevelSelection ||
                    standardMenuState == StandardMenuState._Dispose ) State = LevelConclusionState._Dispose;
            }
            else
            {
                if (standardMenuState == StandardMenuState._Dispose ) State = LevelConclusionState._Dispose;
            }
        }

        public LevelConclusionState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                StateChanged?.Invoke(_state);
                OnInternalStateUpdated();
            }
        }

        protected override void OnInternalStateUpdated()
        {
            Debug.Log($"{gameObject.name} OnInternalStateUpdated :{_state}");
            if (State == LevelConclusionState._Init) Init();
            else if (State == LevelConclusionState.LevelConclusion) SubView();
            else if (State == LevelConclusionState.AfterLevelConclusion)
            {
                State = LevelConclusionState._Dispose;
                AfterLevelConclusion_ViewsClosed?.Invoke();
            }
        }

        private void SubView()
        {
            if (GLModule.Instance.lastLevelConclusionType == GL.Data.LevelConclusionType.Completed) 
                State = LevelConclusionState.LevelComplete;
            else if (GLModule.Instance.lastLevelConclusionType == GL.Data.LevelConclusionType.Failed)
                State = LevelConclusionState.LevelFailed;
        }
    }
}
