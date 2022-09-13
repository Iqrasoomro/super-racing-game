using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;

namespace ArcadianLab.SimFramework.Utils
{
    //[DefaultExecutionOrder((int)ExecutionOrder.BeforeManagers)]
    public class UtilsManager : Manager
    {
        public static UtilsManager Instance;
        public SceneManager_ sceneManager;
        public LoadingOverlay loadingOverlay;
        public PopupMessage popupMessage;

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
            Assert.IsNotNull(sceneManager);
            Assert.IsNotNull(loadingOverlay);
            Assert.IsNotNull(popupMessage);
            sceneManager.Init();
            loadingOverlay.Init();
            popupMessage.Init();
        }
    }
}
