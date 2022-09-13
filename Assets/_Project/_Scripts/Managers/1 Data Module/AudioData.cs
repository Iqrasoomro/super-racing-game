using System;
using System.Collections.Generic;

namespace ArcadianLab.SimFramework.Data
{
    [Serializable]
    public class AudioData
    {
        public bool isMusicOn;
        public bool isSoundOn;

        public AudioData() => this.isMusicOn = this.isSoundOn = false;

        public AudioData(bool isMusicOn, bool isSoundOn)
        {
            this.isMusicOn = isMusicOn;
            this.isSoundOn = isSoundOn;
        }

        public void EnableAudio(bool isMusicOn, bool isSoundOn)
        {
            this.isMusicOn = isMusicOn;
            this.isSoundOn = isSoundOn;
        }
    }
}
