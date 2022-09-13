using System;
using System.Collections.Generic;

namespace ArcadianLab.SimFramework.Data
{
    public static class Constants
    {
        public static class Tags
        {
            public const string LevelToggler = "Level Toggler";
        }

        public const string Homepage       = "https://github.com/scarspeed";
        public const string GameKeyword    = "SS";
        public const string ShareText      = "\nCheck out my NFT!";
        public const string ShareLink      = "\nhttps://github.com/scarspeed/scar-speed-documentation/wiki/Introduction";

		public static class Audio
        {
            public const float defaultVolumeSfx = 1.0f;
            public const float defaultVolumeMusic = 0.5f;
        }

        public static class Data
        {
            public const string gameTitle = "Game Title";
            public const string gameSubTitle = "Game Subtitle Here";

            // Levels
            public const int maxChapters = 3;
            public const int maxLevels = 4;
            public const int minPickables = 3;
            public static Dictionary<LevelState, string> levelLockedMessage = new Dictionary<LevelState, string>
            {
                { LevelState.None, "-"},
                { LevelState.Unlocked_Played, "-"},
                { LevelState.Unlocked_Current, "-"},
                { LevelState.Locked_Soft, "Please watch an Ad to unlock this level"},
                { LevelState.Locked_Hard, "Please complete current level to play this one \nor purchase inapp to unlock all level"},
            };

            public static Dictionary<ChapterState, string> chapterLockedMessage = new Dictionary<ChapterState, string>
            {
                { ChapterState.None, "-"},
                { ChapterState.Unlocked_Played, "-"},
                { ChapterState.Unlocked_Current, "-"},
                { ChapterState.Locked_Hard, "Please complete current chapter to play this one"},
                { ChapterState.Locked_ComingSoon, "More exciting chapters coming soon"},
            };
        }
    }
}