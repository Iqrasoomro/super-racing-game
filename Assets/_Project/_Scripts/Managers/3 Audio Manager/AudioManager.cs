using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;
using ArcadianLab.SimFramework.Data;

namespace ArcadianLab.SimFramework.Audio
{
	[Serializable]
	public enum AudioType
	{
		Sound,
		Music
	};

	public class AudioManager : Manager
	{
		public static AudioManager Instance;
		[SerializeField] private MusicManager music;
		[SerializeField] private SoundManager sound;

		void Awake() => Instantiate();

		public override void Instantiate()
		{
			if (Instance != null)
				Destroy(this.gameObject);
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
			Assert.IsNotNull(this.sound);
			Assert.IsNotNull(this.music);
			Instance = this;
			sound.Init();
			music.Init();
		}

		public void Toggle(AudioType type)
		{
			if (type == AudioType.Sound) this.sound.Toggle();
			else this.music.Toggle();
		}

		public void Play(SfxType soundEffect_Type)
		{
			if(DataManager.Instance.data.audioData.isSoundOn) sound.Play(soundEffect_Type);
		}

		public void Play(MusicType musicType) 
		{
			if (DataManager.Instance.data.audioData.isMusicOn) music.Play(musicType);
		}
    }
}
	

