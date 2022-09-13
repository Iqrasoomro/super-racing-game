using System;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Data;

namespace ArcadianLab.SimFramework
{
    public enum GameManagerState
    {
        None,
        Init,
        NGL,
        GL,
        Dispose
    }

    [DefaultExecutionOrder((int)ExecutionOrder.BeforeSystem)]
    public class GameManager : Manager
    {
        public static GameManager Instance;
        [SerializeField] private GameManagerState _state;
        public static event Action<GameManagerState> GameStateChanged;

        void Awake() => Instantiate();

        public override void Instantiate()
        {
            if (Instance != null) Destroy(this.gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                Init();
                Instantiate_Callback();
            }
        }

        private void OnEnable() => GLModule.AfterLevelConclude += delegate { State = GameManagerState.NGL; };


        private void OnDisable() => GLModule.AfterLevelConclude -= delegate { State = GameManagerState.NGL; };


        public override void Init() => _state = GameManagerState.None;
        public GameManagerState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                if (!IsValid(value)) return;
                _state = value;
                Debug.Log($"GameState :{_state}");
                GameStateChanged?.Invoke(_state);
                OnInternalStateUpdated();
            }
        }

        // GameManager FSM
        private bool IsValid(GameManagerState newState)
        {
            bool result = false;
            if ((_state == GameManagerState.None && newState == GameManagerState.Init) ||
                (_state == GameManagerState.Init && newState == GameManagerState.NGL)||
                (_state == GameManagerState.NGL && newState == GameManagerState.GL)||
                (_state == GameManagerState.GL && newState == GameManagerState.NGL))
                result = true;
            return result;
        }

        protected void OnInternalStateUpdated()
        {
            Debug.Log($"{gameObject.name} OnInternalStateUpdated :{_state}");
            if (State == GameManagerState.Init) State = GameManagerState.NGL;
        }
    }
}
