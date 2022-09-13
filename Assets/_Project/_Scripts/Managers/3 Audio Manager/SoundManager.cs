using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;
using ArcadianLab.SimFramework.Data;
using ArcadianLab.SimFramework.Settings;

namespace ArcadianLab.SimFramework.Audio
{
	[Serializable]
	public enum SfxType
	{
		Tap,
		Success,
		Fail,
		Null,
	}

	public class SoundManager : Object_
	{
		private AudioSource audioSource;
		[SerializeField] private AudioClip[] soundEffects;

		public override void Init()
		{
			audioSource = this.gameObject.GetComponent<AudioSource>();
			Assert.IsNotNull(audioSource);
			Assert.AreEqual(soundEffects.Length, Enum.GetNames(typeof(SfxType)).Length - 1);
			audioSource.volume = Constants.Audio.defaultVolumeSfx;
			audioSource.loop = false;
		}

		private void OnEnable() => SettingsCoreView.EnableSound += EnableSound;
		private void OnDisable() => SettingsCoreView.EnableSound -= EnableSound;

		public void Toggle() => Save(!DataManager.Instance.data.audioData.isSoundOn);

		public void Play(SfxType sfxType)
		{
			if (sfxType == SfxType.Null || audioSource.isPlaying) return;
			this.audioSource.PlayOneShot(soundEffects[(int)sfxType]);
		}

		private void EnableSound(bool value)
        {
			this.audioSource.mute = !value;
			Save(value);
		}

		private void Save(bool value)
		{
			DataManager.Instance.data.audioData.isSoundOn = value;
			DataManager.Instance.Save();
		}
	}
}
