using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.NGL;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework
{
    public enum RootState
    {
        None,
        _Init,
        Loading,
        LoadingComplete,
        _Dispose,
    }

    /// <summary>
    /// Root doesn't have a view manager despite having a view because at the time of
    /// loading core architecture, abstract classes may not function or loaded properly
    /// causing missed events for view manager. Therefore it toggles it's own view as as exception
    /// </summary>
    //[DefaultExecutionOrder((int)ExecutionOrder.Root)]
    public class Root : SingletonManagerWithHomeView
    {
        public static Root Instance;
        public static event Action<Enum> StateChanged;
        [SerializeField] private RootState _state;
        private List<GameObject> _managersList;
        private bool isLocked;
        public DataManager dataManager;
        public GameManager gameManager;
        public NGLModule nglModule;
        public GLModule glModule;

        void Awake() => Instantiate();

        public override void Instantiate()
        {
            if (Instance != null) Destroy(this.gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                Init();
            }
        }

        public new void Instantiate_Callback() { } // For Safety. No Instantiation Callback for Root

        public override void Init()
        {
            isLocked = false;
            Assert.IsNotNull(dataManager);
            Assert.IsNotNull(gameManager);
            Assert.IsNotNull(nglModule);
            Assert.IsNotNull(glModule);
            _managersList = new List<GameObject>();
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (State == RootState.None) State = RootState._Init;
        }

        protected override void OnParentManager_StateUpdated(Enum type) { } // No Parent for Root

        public RootState State
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
            if (State == RootState._Init) State = RootState.Loading;
            else if (State == RootState.Loading)
            {
                ProjectSettings();
                StartCoroutine(LoadManagers());
            }
            else if (State == RootState.LoadingComplete) State = RootState._Dispose;
            else if (State == RootState._Dispose) GameManager.Instance.State = GameManagerState.Init;
        }
        
        private void ProjectSettings()
        {
            Application.targetFrameRate = 60; 
            #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
            #elif UNITY_ANDROID || PLATFORM_ANDROID
            Debug.unityLogger.logEnabled = false;
            #endif 
        }

        IEnumerator LoadManagers()
        {
            yield return LoadManager(dataManager, dataManager.gameObject);
            yield return LoadManager(gameManager, gameManager.gameObject);
            yield return LoadManager(nglModule, nglModule.gameObject);
            yield return LoadManager(glModule, glModule.gameObject);
            yield return new WaitForSeconds(1f);
            State = RootState.LoadingComplete;
        }

        IEnumerator LoadManager(IManager manager, GameObject gameObject)
        {
            if (this.isLocked) yield return null;
            this.isLocked = true;

            Assert.IsNotNull(gameObject);
            Assert.IsNotNull(manager);

            Debug.Log($"Loading {gameObject.name}");
            var managerObj = Instantiate(gameObject, transform);
            _managersList.Add(managerObj);

            yield return new WaitUntil(() => !this.isLocked);
        }

        public void ManagerInstantiation_Callback() => this.isLocked = false;

        private void OnDestroy()
        {
            if (_managersList == null) return;
            foreach (GameObject managerObject in _managersList.ToArray())
            {
                _managersList.Remove(managerObject);
                DestroyImmediate(managerObject.gameObject);
            }
            _managersList.Clear();
        }
    }
}
