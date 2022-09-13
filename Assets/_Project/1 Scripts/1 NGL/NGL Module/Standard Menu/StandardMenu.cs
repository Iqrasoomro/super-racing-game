using System;
using UnityEngine;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.NGL.Utils;

namespace ArcadianLab.SimFramework.NGL.StdMenu
{
    public enum StandardMenuState
    {
        None,
        _Init,
        HudHomeView, // +MainMenuView --> NGL
        ChapterSelection,
        LevelSelection,
        LevelConclusion,
        _Dispose,
    }

    //[DefaultExecutionOrder((int)ExecutionOrder.Managers)]
    public class StandardMenu : SubManagerWithHomeView
    {
        [SerializeField] private StandardMenuState _state;
        public static event Action<Enum> StateChanged;

        public override void Init()
        {
            base.Init();
            //UtilsManager.Instance.sceneManager.LoadScene(DataManager.Instance.data.gameData.userSelectedChapter.id + 1);
        }

        private void OnEnable()
        {
            NGLModule.StateChanged += OnParentManager_StateUpdated;
            MainMenuView.OnPlayButtonClicked += delegate { State = StandardMenuState.ChapterSelection; };
            HudHomeView.OnBackButtonClicked += delegate { --State; };
            ChapterSelectionView.OnNextButtonClicked += delegate { State = StandardMenuState.LevelSelection; };
            LevelSelectionView.PlayLevel += delegate { GameManager.Instance.State = GameManagerState.GL;};
            LevelConclusion.AfterLevelConclusion_ViewsClosed += delegate { State = StandardMenuState.LevelSelection; }; // Ch?
        }

        private void OnDisable()
        {
            NGLModule.StateChanged -= OnParentManager_StateUpdated;
            MainMenuView.OnPlayButtonClicked -= delegate { State = StandardMenuState.ChapterSelection; };
            HudHomeView.OnBackButtonClicked -= delegate { --State; };
            ChapterSelectionView.OnNextButtonClicked -= delegate { State = StandardMenuState.LevelSelection; };
            LevelSelectionView.PlayLevel -= delegate { GameManager.Instance.State = GameManagerState.GL; };
            LevelConclusion.AfterLevelConclusion_ViewsClosed -= delegate { State = StandardMenuState.LevelSelection; }; // Ch?
        }

        protected override void OnParentManager_StateUpdated(Enum type)
        {
            NGLState nglState = (NGLState)type;
            if (State == StandardMenuState.None)
            {
                if (nglState == NGLState._Init) State = StandardMenuState._Init;
            }
            else if (State == StandardMenuState._Init || State == StandardMenuState._Dispose)
            {
                if (nglState == NGLState.NGL) State = StandardMenuState.HudHomeView;
            }
            /*else if (State == StandardMenuState.SignUpHome || State == StandardMenuState.CheckEmail)
            {
                if (nglState == NGLState.SignIn) State = StandardMenuState._Dispose;
            }*/
            else
            {
                if (nglState == NGLState._Dispose ) State = StandardMenuState._Dispose;
            }
        }

        public StandardMenuState State
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
            if (State == StandardMenuState._Init) Init();
            else if (State == StandardMenuState.LevelSelection)
                UtilsManager.Instance.sceneManager.LoadScene(DataManager.Instance.data.gameData.userSelectedChapter.id + 1);
        }  
    }
}
