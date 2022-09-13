using System;
using UnityEngine;

namespace ArcadianLab.SimFramework.Core
{
    public enum AppState
    {
        None,
        Authentication, // +Wallet
        InventorySetup, // Inventory management, rendering etc
        Experience,     // Inventory has been setup, media features being used, running state
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.BeforeManagers)]
    public class AppManager : Manager
    {
        public static AppManager Instance;
        [SerializeField] private AppState _state;
        public static event Action<AppState> AppStateChanged;

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

        public override void Init() => _state = AppState.None;
        public AppState State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                if (!IsValid(value)) return;
                _state = value;
                Debug.Log($"AppState :{_state}");
                AppStateChanged?.Invoke(_state);
            }
        }

        // AppManager FSM
        private bool IsValid(AppState newState)
        {
            bool result = false;
            if ((_state == AppState.None && newState == AppState.Authentication) ||
                (_state == AppState.Authentication && newState == AppState.InventorySetup) ||
                (_state == AppState.InventorySetup && newState == AppState.Experience))
            result = true;
            return result;
        }
    }
}
