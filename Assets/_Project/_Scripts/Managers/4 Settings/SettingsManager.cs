using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;

namespace ArcadianLab.SimFramework.Settings
{
    public enum SettingsState
    {
        None,
        _Init,
        InvisibleHome,
        SidePanelHome,
        SettingsCore,
        Account,
        Hide,
        _Dispose,
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.BeforeManagers)]
    public class SettingsManager : SingletonManagerWithHomeView
    {
        public static SettingsManager Instance;
        public static event Action<Enum> StateChanged;
        [SerializeField] private SettingsState _state;

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

        public override void Init()
        {
        }

        /*private void OnEnable() => CarsManager.StateChanged += OnParentManager_StateUpdated;
        private void OnDisable() => CarsManager.StateChanged -= OnParentManager_StateUpdated;*/

        protected override void OnParentManager_StateUpdated(Enum type) 
        {
            /*CarsManagerState carsManagerState = (CarsManagerState)type;
            if (State == SettingsState.None)
            {
                if (carsManagerState == CarsManagerState._Init) State = SettingsState._Init;
            }
            else if (carsManagerState == CarsManagerState._Dispose) State = SettingsState._Dispose;*/
        }

        public SettingsState State
        {
            get { return _state; } 
            set
            {
                _state = value;
                StateChanged?.Invoke(_state);
                OnInternalStateUpdated();
            }
        }

        protected override void OnInternalStateUpdated()
        {
            if (State == SettingsState._Init) Init();
            else if (State == SettingsState.Hide) State = SettingsState.InvisibleHome;
        }
    }
}
