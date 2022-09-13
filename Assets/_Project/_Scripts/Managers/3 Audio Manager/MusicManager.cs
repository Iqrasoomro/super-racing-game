using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.Settings;

namespace ArcadianLab.SimFramework.Audio
{
	[Serializable]
	public enum MusicType
	{
		StandardLoop,
		Null,
	}

	[RequireComponent(typeof(AudioSource))]
	public class MusicManager : Object_
	{
		private AudioSource audioSource;
		[SerializeField] private AudioClip[] loops;
		private const MusicType defaultLoopType = MusicType.StandardLoop;

		public override void Init()
		{
			audioSource = this.gameObject.GetComponent<AudioSource>();
			Assert.IsNotNull(audioSource);
			Assert.AreEqual(loops.Length, Enum.GetNames(typeof(MusicType)).Length - 1);
			audioSource.volume = Constants.Audio.defaultVolumeMusic;
			audioSource.loop = true;
			if (DataManager.Instance.data.audioData.isMusicOn) Play();
		}

		private void OnEnable() => SettingsCoreView.EnableMusic += OnEnableMusic;
		private void OnDisable() => SettingsCoreView.EnableMusic -= OnEnableMusic;

		public void Toggle()
		{
			bool value = !DataManager.Instance.data.audioData.isMusicOn;
			audioSource.mute = !value;
			Save(value);
			if (value)
			{
				if (!audioSource.isPlaying) audioSource.Play();
			}
		}

		public void Play(MusicType musicType = defaultLoopType)
		{
			if (musicType == MusicType.Null) return;
			this.audioSource.clip = loops[(int)musicType];
			this.audioSource.Play();
		}

		private void OnEnableMusic(bool value)
        {
			if (value)
			{
				if (this.audioSource.isPlaying) this.audioSource.UnPause();
				else Play();

			}
			else this.audioSource.Pause();
			Save(value);
		}

		private void Save(bool value)
        {
			DataManager.Instance.data.audioData.isMusicOn = value;
			DataManager.Instance.Save();
		}
		
    }
}
