using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance;

	public Sound[] Sounds = new Sound[9]
	{
		new Sound(SoundNames.UnresolvedGamePlay),
		new Sound(SoundNames.ResolvedGamePlay),
		new Sound(SoundNames.StartGame),
		new Sound(SoundNames.CompletedGame),
		new Sound(SoundNames.ValidHorizontalMovement),
		new Sound(SoundNames.ValidVerticalMovement),
		new Sound(SoundNames.InvalidMovement),
		new Sound(SoundNames.InvalidOperation),
		new Sound(SoundNames.PlayerEffect)
	};

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		InitializeSounds();
	}

	private void Start()
	{
		Play(SoundNames.UnresolvedGamePlay);
	}

	private void InitializeSounds()
	{
		foreach (var sound in Sounds)
		{
			if (sound.Clip != null)
			{
				sound.Source = gameObject.AddComponent<AudioSource>();
				sound.Source.clip = sound.Clip;

				sound.Source.volume = sound.Volume;
				sound.Source.loop = sound.Loop;
			}
		}
	}

	private Sound Find(string name)
	{
		return Array.Find(Sounds, sound => sound.Name == name);
	}
	public void Play(string name)
	{
		Sound sound = Find(name);

		if (sound == null || sound.Source == null) return;

		sound.Source.volume = sound.Volume;
		sound.Source.Play();
	}

	public void Stop(string name)
	{
		Sound sound = Find(name);

		if (sound == null || sound.Source == null) return;

		sound.Source.Stop();
	}

	public void DOFadeOutTo(string offSound, string onSound, float duration)
	{
		Sound offMusic = Find(offSound);
		Sound onMusic = Find(onSound);

		if (offMusic == null || offMusic.Source == null) return;

		offMusic.Source.DOFade(0, duration).OnComplete(() => {
			offMusic.Source.Stop();
			if (onMusic == null || onMusic.Source == null) return;
			onMusic.Source.volume = 0;
			onMusic.Source.Play();
			onMusic.Source.DOFade(onMusic.Volume, duration);
		});
	}
}
