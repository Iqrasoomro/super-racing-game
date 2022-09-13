using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.NGL.Audio;
using ArcadianLab.SimFramework.NGL.StdMenu;
using ArcadianLab.SimFramework.NGL.Utils;

namespace ArcadianLab.SimFramework
{
    public enum NGLState
    {
        None,
        _Init,
        NGL,
        _Dispose,
    }
    
    //[DefaultExecutionOrder((int)ExecutionOrder.Managers)]
    public class NGLModule : ModuleManager
    {
        public static NGLModule Instance;
        public static event Action<Enum> StateChanged;
        [SerializeField] private NGLState _state;
        public StandardMenu standardMenu;
        public UtilsManager utilsManager;
        public AudioManager audioManager;
        // Stub stub;

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
            Debug.Log($"{gameObject.name} Init");
            Assert.IsNotNull(standardMenu);
            Assert.IsNotNull(utilsManager);
            Assert.IsNotNull(audioManager);
        }

        private void OnEnable()
        {
            GameManager.GameStateChanged += OnGameManager_StateChanged;
            GLModule.AfterLevelConclude += delegate { standardMenu.State = StandardMenuState.LevelConclusion; };
        }

        private void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameManager_StateChanged;
            GLModule.AfterLevelConclude -= delegate { standardMenu.State = StandardMenuState.LevelConclusion; };
        }

        protected override void OnGameManager_StateChanged(GameManagerState appState)
        {
            if (appState == GameManagerState.Init) State = NGLState._Init;
            else if (appState == GameManagerState.NGL) State = NGLState.NGL;
            else if (appState == GameManagerState.GL || appState == GameManagerState.Dispose) State = NGLState._Dispose;
        }

        public NGLState State
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
            if (State == NGLState._Init) Init();
            //else if (State == AuthState.AuthenticationSuccessful) State = AuthState.Wallet;
            //else if (State == AuthState._Dispose) AppManager.Instance.State = AppState.InventorySetup;
        }
    }
}
