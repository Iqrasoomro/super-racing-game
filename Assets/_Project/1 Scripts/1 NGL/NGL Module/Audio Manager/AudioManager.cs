using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Data;


namespace ArcadianLab.SimFramework.NGL.Audio
{
	[Serializable]
	public enum AudioType
	{
		Sound,
		Music
	};

	// Stateless submanager as per needs
	public class AudioManager : SubManager
	{
		public static AudioManager Instance;
		[SerializeField] private MusicManager music;
		[SerializeField] private SoundManager sound;

		public override void Init()
		{
			Assert.IsNotNull(this.sound);
			Assert.IsNotNull(this.music);
			Instance = this;
			sound.Init();
			music.Init();
		}

		protected override void OnParentManager_StateUpdated(Enum type)
		{
			NGLState nglState = (NGLState)type;
			if (nglState == NGLState._Init) Init();
		}

		protected override void OnInternalStateUpdated() { }

		public void Toggle(AudioType type)
		{
			if (type == AudioType.Sound) this.sound.Toggle();
			else this.music.Toggle();
		}

		public void Play(SfxType soundEffect_Type)
		{
			if (DataManager.Instance.data.audioData.isSoundOn) sound.Play(soundEffect_Type);
		}

		public void Play(MusicType musicType)
		{
			if (DataManager.Instance.data.audioData.isMusicOn) music.Play(musicType);
		}
	}
}
	

