using System;

namespace ArcadianLab.SimFramework.Data
{
    [Serializable]
    public class Data
    {
        public AudioData audioData;

        public Data()
        {
            this.audioData = new AudioData();
        }
    }
}
