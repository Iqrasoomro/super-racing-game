using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.GL;
using ArcadianLab.SimFramework.GL.Data;
using ArcadianLab.SimFramework.GL.Systems;
using ArcadianLab.SimFramework.NGL.Utils;

namespace ArcadianLab.SimFramework
{
    public enum GLState
    {
        None,
        _Init,
        Systems,
        Entities,
        Gameplay_Start,
        Gameplay_Stop,
        _Dispose,
    }

    [DefaultExecutionOrder((int)ExecutionOrder.System)]
    public class GLModule : ModuleManager
    {
        public static GLModule Instance;
        public static event Action<Enum> StateChanged;
        public static event Action AfterLevelConclude;
        [SerializeField] private GLState _state;
        public SystemsManager systemsManager;
        public EntitiesManager entitiesManager;
        public Chapter userSelectedChapter;
        public LevelConclusionType lastLevelConclusionType;

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
            Assert.IsNotNull(systemsManager);
            Assert.IsNotNull(entitiesManager);
            userSelectedChapter = DataManager.Instance.data.gameData.userSelectedChapter;
        }

        private void OnEnable()
        {
            GameManager.GameStateChanged += OnGameManager_StateChanged;
            GameStateHandler.LevelConclude += LevelConclude_GameplayStop;
        }
        private void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameManager_StateChanged;
            GameStateHandler.LevelConclude -= LevelConclude_GameplayStop;
        }

        protected override void OnGameManager_StateChanged(GameManagerState gameManagerState)
        {
            if (gameManagerState == GameManagerState.Init) State = GLState._Init;
            else if (gameManagerState == GameManagerState.GL) State = GLState.Systems; // Starts with loading all systems
            else if (gameManagerState == GameManagerState.NGL || gameManagerState == GameManagerState.Dispose)
            {
                if(State == GLState.Gameplay_Start)
                {
                    State = GLState.Gameplay_Stop;
                    State = GLState._Dispose;
                }
                else if (State == GLState.Gameplay_Stop)
                {
                    State = GLState._Dispose;
                }
            }
        }

        public GLState State
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
            if (State == GLState._Init) Init();
            else if (State == GLState.Systems) State = GLState.Entities;
            else if (State == GLState.Entities) OnEntitiesInit();

        }

        private void OnEntitiesInit()
        {
            UtilsManager.Instance.popupMessage.Show("Objective: collect all pickables, avoid enemy", 1f,
                    delegate { State = GLState.Gameplay_Start; });
        }

        private void LevelConclude_GameplayStop(LevelConclusionType type)
        {
            State = GLState.Gameplay_Stop;
            lastLevelConclusionType = type;
            AfterLevelConclude?.Invoke();
        }

        private void OnGUI()
        {
            GUI.skin.label.fontSize = 20;
            GUI.skin.button.fontSize = 20;

            if (GameManager.Instance.State != GameManagerState.GL) return;
            if (userSelectedChapter == null) return;
            GUILayout.Label($"Chapter = {userSelectedChapter.id + 1}");
            GUILayout.Label($"Level = {userSelectedChapter.userSelectedLevel.id + 1}");
            GUILayout.Label($"Max Pickables = {userSelectedChapter.userSelectedLevel.maxPickables}");
            if (systemsManager != null)  GUILayout.Label($"{systemsManager.State}");

            //if (GUILayout.Button("Back")) GameManager.Instance.State = GameManagerState.NGL;
        }
    }
}
