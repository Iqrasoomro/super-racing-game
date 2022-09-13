using System;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Systems;
using ArcadianLab.SimFramework.GL.Abstract;
using Cube.MiniGame.Data;

namespace ArcadianLab.SimFramework.GL
{
    public enum SystemsManagerState
    {
        None,
        _Init,
        InitSystems,
        StartSystems,
        StopSystems,
        ClearSystems,
        _Dispose
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Data)]
    public class SystemsManager : SubManager
    {
        public static SystemsManager Instacne;
        private SystemsManagerState _state;
        public static event Action<SystemsManagerState> StateChanged;
        public List<GameSystem> systems;

        public override void Init() => Instacne = this;

        private void OnEnable()
        {
            GLModule.StateChanged += OnParentManager_StateUpdated;
            //GameStateHandler.LevelConclude += OnLevelConcluded;
            
        }
        private void OnDisable()
        {
            GLModule.StateChanged -= OnParentManager_StateUpdated;
            //GameStateHandler.LevelConclude -= OnLevelConcluded;
        }

        public SystemsManagerState State
        {
            get { return _state; }
            private set
            {
                if (_state == value) return;
                _state = value;
                StateChanged?.Invoke(_state);
                OnInternalStateUpdated();
                Debug.Log($"--- SystemState :{_state} ---");
            }
        }

        protected override void OnParentManager_StateUpdated(Enum type)
        {
            GLState glState = (GLState)type;
            if (State == SystemsManagerState.None)
            {
                if (glState == GLState._Init) State = SystemsManagerState._Init;
            }
            else if (State == SystemsManagerState._Init || State == SystemsManagerState._Dispose)
            {
                if (glState == GLState.Systems) State = SystemsManagerState.InitSystems;
            }
            else if (State == SystemsManagerState.InitSystems || State == SystemsManagerState.StopSystems)
            {
                if (glState == GLState.Gameplay_Start)
                {
                    State = SystemsManagerState.ClearSystems;
                    State = SystemsManagerState.StartSystems;
                }
            }
            else if (State == SystemsManagerState.StartSystems)
            {
                if (glState == GLState.Gameplay_Stop)
                {
                    State = SystemsManagerState.StopSystems;
                    State = SystemsManagerState._Dispose;
                }
            }
            else
            {
                if (glState == GLState._Dispose) State = SystemsManagerState._Dispose;
            }
        }

        protected override void OnInternalStateUpdated()
        {
            Debug.Log($"{gameObject.name} OnInternalStateUpdated :{_state}");
            if (State == SystemsManagerState._Init) Init();
        }
    }
}
