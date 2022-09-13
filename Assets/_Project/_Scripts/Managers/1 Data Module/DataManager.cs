using UnityEngine;
using ArcadianLab.SimFramework.Core;
using ArcadianLab.SimFramework.Settings;

namespace ArcadianLab.SimFramework.Data
{
    //[DefaultExecutionOrder((int)ExecutionOrder.Data)]
    public class DataManager : Manager
    {
        public static DataManager Instance;
        public Data data;
        private const string key = "ssnfts";

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

        public override void Init() => Load();

        private void Load()
        {
            if (ES3.KeyExists(key)) this.data = ES3.Load<Data>(key);
            else
            {
                DefaultData.Overwrite();
                Save(); // Saving the over-written (fed) data at the first time run
            }
        }

        public void Save() => ES3.Save(key, data);

        private void OnEnable()
        {
            /*MediaManagerCore.NewMediaObjectCreated += data.mediaData.SaveMedia;
            MediaManager.MediaObjectDeleted += data.mediaData.DeletMedia;*/
        }

        private void OnDisable()
        {
            /*MediaManagerCore.NewMediaObjectCreated -= data.mediaData.SaveMedia;
            MediaManager.MediaObjectDeleted -= data.mediaData.DeletMedia;*/
        }

        public void Reset()
        {
            if (ES3.KeyExists(key))
            {
                ES3.DeleteKey(key);
                ES3.DeleteFile(key);
                this.Load();
                Save();
            }
        }
    }
}
