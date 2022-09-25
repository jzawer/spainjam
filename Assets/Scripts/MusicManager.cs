using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance;

	public AudioMixerGroup mixer;
	public Sound[] Sounds = new Sound[18]
	{
		new Sound(SoundNames.Player_Move),
		new Sound(SoundNames.Player_Waiting),
		new Sound(SoundNames.Interaction_Horizontal),
		new Sound(SoundNames.Interaction_Vertical),
		new Sound(SoundNames.Invalid_Action),
		new Sound(SoundNames.Number_Up),
		new Sound(SoundNames.Number_Down),
		new Sound(SoundNames.Level_Start),
		new Sound(SoundNames.Player_Get_100),
		new Sound(SoundNames.Player_Loose_100),
		new Sound(SoundNames.Level_Completed),
		new Sound(SoundNames.StartMenu_Play),
		new Sound(SoundNames.StartMenu_Hover),
		new Sound(SoundNames.StartMenu_Quit),
		new Sound(SoundNames.StartMenu_Start),
		new Sound(SoundNames.StartMenu_Loop),
		new Sound(SoundNames.Gameplay_UnresolvedLoop),
		new Sound(SoundNames.Gameplay_ResolvedLoop)
};

	void Awake()
	{
		if (Instance == null)
		{
			InitializeSounds();
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		//Play(SoundNames.UnresolvedGamePlay);
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

				if (mixer != null)
					sound.Source.outputAudioMixerGroup = mixer;
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

		if (!sound.Source.isPlaying)
			sound.Source.Play();
	}

	public IEnumerator PlayDelayedBySound(string current, string next)
    {
		Sound currentSound = Find(current);

		if (currentSound == null || currentSound.Source)
			yield break;

		yield return new WaitForSeconds(currentSound.Clip.length);
		this.Play(next);
	}

	public void Stop(string name)
	{
		Sound sound = Find(name);

		if (sound == null || sound.Source == null) return;

		sound.Source.Stop();
	}

	public void ChangeClipTo(string offSound, string onSound, float duration)
	{
		Sound offMusic = Find(offSound);
		Sound onMusic = Find(onSound);

		if (offMusic == null || offMusic.Source == null || onMusic == null || onMusic.Source == null || onMusic.Source.isPlaying) return;

		onMusic.Source.time = offMusic.Source.time;
		offMusic.Source.Stop();
		onMusic.Source.Play();

		/*offMusic.Source.DOFade(0, duration).OnComplete(() => {
			offMusic.Source.Stop();
			if (onMusic == null || onMusic.Source == null) return;
			onMusic.Source.volume = 0;
			onMusic.Source.Play();
			onMusic.Source.DOFade(onMusic.Volume, duration);
		});*/
	}
}
