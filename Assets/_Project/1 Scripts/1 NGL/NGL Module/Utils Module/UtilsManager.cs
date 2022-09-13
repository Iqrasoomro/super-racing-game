using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ArcadianLab.SimFramework.NGL.Utils
{
    // Stateless Manager as per needs
    //[DefaultExecutionOrder((int)ExecutionOrder.BeforeManagers)]
    public class UtilsManager : SubManager
    {
        public static UtilsManager Instance;
        public SceneManager_ sceneManager;
        public LoadingOverlay loadingOverlay;
        public PopupMessage popupMessage;

        public override void Init()
        {
            Assert.IsNotNull(sceneManager);
            Assert.IsNotNull(loadingOverlay);
            Assert.IsNotNull(popupMessage);
            Instance = this;                    // For convenience, exception
            sceneManager.Init();
            loadingOverlay.Init();
            popupMessage.Init();
        }

        private void OnEnable() => NGLModule.StateChanged += OnParentManager_StateUpdated;

        private void OnDisable() => NGLModule.StateChanged -= OnParentManager_StateUpdated;

        protected override void OnParentManager_StateUpdated(Enum type)
        {
            NGLState nglState = (NGLState)type;
            if (nglState == NGLState._Init) Init() ;
        }

        protected override void OnInternalStateUpdated() { }
    }
}
