using UnityEngine;
using ArcadianLab.SimFramework.NGL.StdMenu;
using ArcadianLab.SimFramework.GL.Systems;
using ArcadianLab.SimFramework.GL.Data;

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
            ChapterSelection.SelectedChapter += OnChapterSelected;
            LevelSelection.SelectedLevel += OnLevelSelected;
            LevelSelection.ChapterComplete += OnChapterComplete;
            GameStateHandler.LevelConclude += OnLevelConclude;
        }

        private void OnDisable()
        {
            ChapterSelection.SelectedChapter += OnChapterSelected;
            LevelSelection.SelectedLevel -= OnLevelSelected;
            LevelSelection.ChapterComplete -= OnChapterComplete;
            GameStateHandler.LevelConclude -= OnLevelConclude;
        }

        private void OnChapterSelected(Chapter chapter)
        {
            data.gameData.userSelectedChapter = chapter;
            Save();
        }

        private void OnLevelSelected(Level level)
        {
            data.gameData.userSelectedChapter.userSelectedLevel = level;
            Save();
        }

        private void OnLevelConclude(GL.Data.LevelConclusionType conclusion)
        {
            if (conclusion != GL.Data.LevelConclusionType.Completed) return;
            data.gameData.userSelectedChapter.OnLevelComplete();
            Save();
        }

        private void OnChapterComplete()
        {
            data.gameData.OnChapterComplete();
            Save();
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
