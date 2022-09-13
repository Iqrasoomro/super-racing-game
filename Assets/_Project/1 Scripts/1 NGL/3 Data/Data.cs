using System;

namespace ArcadianLab.SimFramework.Data
{
    [Serializable]
    public class Data
    {
        public GameData gameData;
        public AudioData audioData;

        public Data()
        {
            this.gameData = new GameData();
            this.audioData = new AudioData();
        }
    }
}
