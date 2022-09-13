using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcadianLab.SimFramework.Data
{
    public static class DefaultData
    {
        public static void Overwrite()
        {
            /*List<Level> levels = new List<Level>()
            {
                new Level(LevelState.Unlocked_Current, 3),
                new Level(LevelState.Locked_Soft, 4),
                new Level(LevelState.Locked_Hard, 5),
                new Level(LevelState.Locked_Hard, 5),
            };*/

            List<Chapter> chapters = new List<Chapter>()
            {
                new Chapter("Chapter1", ChapterState.Unlocked_Current, Levels()),
                new Chapter("Chapter2", ChapterState.Locked_Hard, Levels()),
                new Chapter("Chapter3", ChapterState.Locked_Hard, Levels()),
                new Chapter("Chapter4", ChapterState.Locked_ComingSoon, Levels()),
            };

            Data data = new Data();
            data.gameData = new GameData(GameAdsState.Ads, chapters);
            data.audioData = new AudioData(true, true);
            DataManager.Instance.data = data;
        }

        private static List<Level> Levels()
        {
            int totalLevels = Constants.Data.maxLevels;
            if (totalLevels < 3) throw new Exception("Levels for a given chapter should be >= 3");
            Level.idCounter = 0;
            List<Level> levels = new List<Level>();
            for (int i = 0; i < totalLevels; i++) levels.Add(new Level(LevelState.Locked_Hard, Constants.Data.minPickables + i));
            levels[0].state = LevelState.Unlocked_Current;
            levels[1].state = LevelState.Locked_Soft;
            return levels;
        }
    }
}
