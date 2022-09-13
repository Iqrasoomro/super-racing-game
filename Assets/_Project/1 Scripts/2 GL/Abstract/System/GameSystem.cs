using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework;
using ArcadianLab.SimFramework.GL.Systems;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Abstract
{

    public enum SystemManagerState
    {
        None,
        _Init,
        Start,
        Stop,
        Clear,
        _Dispose
    }

    [Serializable]
    public abstract class GameSystem : MonoBehaviour, IGameSystem
    {
        public static event Action<Enum> StateChanged;
        public static event Action<Enum> StateChangedForEntities;
        [SerializeField] private SystemManagerState _state;

        public SystemManagerState State
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

        private void OnEnable() => SystemsManager.StateChanged += OnParentStateUpdated;
        private void OnDisable() => SystemsManager.StateChanged -= OnParentStateUpdated;

        public void OnParentStateUpdated(SystemsManagerState gsmState)
        {
            if (State == SystemManagerState.None)
            {
                if (gsmState == SystemsManagerState.InitSystems) State = SystemManagerState._Init;
            }
            else if (State == SystemManagerState._Init || State == SystemManagerState.Stop || State == SystemManagerState._Dispose)
            {
                if (gsmState == SystemsManagerState.StartSystems) State = SystemManagerState.Start;
            }
            else if (State == SystemManagerState.Start)
            {
                if (gsmState == SystemsManagerState.StopSystems) State = SystemManagerState.Stop;
                else if (gsmState == SystemsManagerState._Dispose) { State = SystemManagerState.Stop; State = SystemManagerState._Dispose; }
            }
            else
            {
                if (gsmState == SystemsManagerState._Dispose) State = SystemManagerState._Dispose;
            }
        }

        private void OnInternalStateUpdated()
        {
            Debug.Log($"{gameObject.name} OnInternalStateUpdated :{_state}");
            if (State == SystemManagerState._Init) Init();
            else if (State == SystemManagerState.Start) StartSystem();
            else if (State == SystemManagerState.Stop) StopSystem();
            else if (State == SystemManagerState.Clear) ClearSystem();
            else if (State == SystemManagerState._Dispose) Debug.Log("XXXXXXXXXXXXXXX");
        }

        public virtual void Init() { }

        public virtual void StartSystem() { }

        public virtual void StopSystem() { }

        public virtual void ClearSystem() { }

        public virtual void Dispose() { }

    }
}
