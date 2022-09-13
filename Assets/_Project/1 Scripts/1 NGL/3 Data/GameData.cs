using System;
using System.Collections.Generic;

namespace ArcadianLab.SimFramework.Data
{
    [Serializable]
    public enum LevelState
    {
        None = -1,

        Unlocked_Played,
        Unlocked_Current,
        Locked_Soft,        // Unlock via video ad
        Locked_Hard,
    }

    [Serializable]
    public class Level
    {
        public static int idCounter = 0;
        public int id;
        public LevelState state;
        public int maxPickables;

        public Level()
        {
            this.id = 0;
            this.state = LevelState.None;
            this.maxPickables = 0;
        }

        public Level(LevelState state, int maxPickables)
        {
            if (state == LevelState.Unlocked_Current) idCounter = 0;
            this.id = idCounter++;
            this.state = state;
            this.maxPickables = maxPickables;
        }
    }

    [Serializable]
    public enum ChapterState
    {
        None = -1,

        Unlocked_Played,
        Unlocked_Current,
        Locked_Hard,
        Locked_ComingSoon,
    }

    [Serializable]
    public class Chapter
    {
        public static event Action RequestLevelComplete_;
        public static event Action<Level, Level> AfterLevelCompleted;
        public static event Action LevelRecompleted;
        public static int idCounter = 0;
        public int id;
        public string name;
        public ChapterState state;
        public List<Level> levels;
        public Level currentLevel;
        public Level nextLevel;
        public Level userSelectedLevel;
        public const int maxLevels = Constants.Data.maxLevels;

        public Chapter()
        {
            this.id = 0;
            this.name = "-";
            this.state = ChapterState.None;
            this.levels = new List<Level>();
            this.currentLevel = null;
            this.nextLevel = null;
            this.userSelectedLevel = null;
        }

        public Chapter(string name, ChapterState state, List<Level> levels)
        {
            this.id = idCounter++;
            this.name = name;
            this.state = state;
            this.levels = levels;
            this.currentLevel = levels.Count > 0 ? levels[0] : null;
            this.nextLevel = levels.Count > 1 ? levels[1] : null;
            this.userSelectedLevel = currentLevel;
        }

        public void OnLevelComplete()
        {
            if(DataManager.Instance.data.gameData.IsPlayingCurrentChapter())
            {
                // Playing current chapter & completed a level
                if (userSelectedLevel.id == currentLevel.id)
                {
                    // Completed a current level of current chapter
                    if (currentLevel.state != LevelState.Unlocked_Current) throw new Exception("current level state mismatched");

                    // Completed
                    currentLevel.state = LevelState.Unlocked_Played;

                    // Next
                    if (currentLevel.id + 1 < levels.Count)
                    {
                        levels[currentLevel.id + 1].state = LevelState.Unlocked_Current;
                        currentLevel = levels[currentLevel.id + 1];
                        userSelectedLevel = currentLevel;

                        if (currentLevel.id + 1 < levels.Count)
                        {
                            nextLevel = levels[currentLevel.id + 1];
                            levels[currentLevel.id + 1].state = LevelState.Locked_Soft;
                        }
                        else nextLevel = null;
                    }
                    else currentLevel = null;

                    AfterLevelCompleted?.Invoke(currentLevel, nextLevel);
                }
                else
                {
                    // Completed a previous level of current chapter
                    // Calling a level complete call without checks and reassigning user selected level
                    LevelRecompleted?.Invoke();
                    userSelectedLevel = currentLevel;
                }
            }
            else
            {
                // Played previous chapter & completed a level
                // Calling a level complete call without checks and reassigning user selected level
                LevelRecompleted?.Invoke();
            }
        }

        public bool AllLevelsPlayed()
        {
            bool result = false;
            if (state == ChapterState.Unlocked_Current && 
                currentLevel.id == levels[levels.Count - 1].id &&
                currentLevel.state == LevelState.Unlocked_Played) result = true;
            return result;
        }
    }

    [Serializable]
    public enum GameAdsState
    {
        None,
        Ads,                        // Ads, Locked -> Unlocked chapters, levels 
        NoAds,                      // No Ads, But Locked -> Unlocked chapters, levels 
        NoAds_EverythingUnlocked,   // No Ads, All chapters, All levels unlocked, kill progression
    }

    [Serializable]
    public class GameData
    {
        public static event Action<Chapter, Chapter> AfterChapterComplete;
        public string title = Constants.Data.gameTitle;
        public string subtitle = Constants.Data.gameSubTitle;
        public GameAdsState gameAdsState;
        public List<Chapter> chapters;
        public Chapter currentChapter;
        public Chapter nextChapter;
        public Chapter userSelectedChapter;
        public int totalAvailableChapters;
        public const int maxChapters = Constants.Data.maxChapters;

        public GameData()
        {
            this.gameAdsState = GameAdsState.None;
            this.chapters = new List<Chapter>();
            this.currentChapter = null;
            this.nextChapter = null;
            this.userSelectedChapter = null;
            this.totalAvailableChapters = 0;
        }

        public GameData(GameAdsState gameState, List<Chapter> chapters)
        {
            this.gameAdsState = GameAdsState.None;
            this.chapters = chapters;
            this.totalAvailableChapters = GetTotalAvailableChapters();
            this.currentChapter = totalAvailableChapters > 0 ? chapters[0] : null;
            this.nextChapter = totalAvailableChapters > 1 ? chapters[1] : null;
            SetCurrentChapter_AsUserSelectedChapter();
            ValidateChapters(chapters);
        }

        private void ValidateChapters(List<Chapter> chapters)
        {
            if (chapters == null) throw new Exception("Null chapters list");
            if (chapters.Count <= 0) throw new Exception("Empty chapters list");
            if (totalAvailableChapters <= 0) throw new Exception("Total available/playable chapters = 0");
            if(chapters[0].state != ChapterState.Unlocked_Current) 
                throw new Exception("First chapter state should be Unlocked_Current");
        }

        private int GetTotalAvailableChapters()
        {
            int count = 0;
            foreach (Chapter chapter in chapters) if (chapter.state != ChapterState.Locked_ComingSoon) count++;
            return count;
        }

        public void OnChapterComplete()
        {
            if (userSelectedChapter.id != currentChapter.id) return;
            if (!(currentChapter.state == ChapterState.Unlocked_Current ||
                currentChapter.state == ChapterState.Unlocked_Played)) return;

            // Completed
            currentChapter.state = ChapterState.Unlocked_Played;

            // Next
            if (currentChapter.id + 1 < totalAvailableChapters)
            {
                chapters[currentChapter.id + 1].state = ChapterState.Unlocked_Current;
                currentChapter = chapters[currentChapter.id + 1];
                SetCurrentChapter_AsUserSelectedChapter();

                if (currentChapter.id + 1 < totalAvailableChapters) nextChapter = chapters[currentChapter.id + 1];
                else nextChapter = null;
            }
            //else currentChapter = null;
            AfterChapterComplete?.Invoke(currentChapter, nextChapter);
        }

        public bool AllChaptersPlayed()
        {
            bool result = false;
            if (currentChapter.id == chapters[totalAvailableChapters - 1].id &&
                currentChapter.state == ChapterState.Unlocked_Played) result = true;
            return result;
        }

        public void SetCurrentChapter_AsUserSelectedChapter() => userSelectedChapter = currentChapter;

        public bool IsPlayingCurrentChapter() { return userSelectedChapter.id == currentChapter.id; }
    }
}
