using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public AudioSource MusicSource;
	public AudioSource EffectSource;

	public static SoundManager Instance = null;

	public AudioClip[] NoteClips;
	public AudioClip ErrorSound;
	
	// Use this for initialization
	private void Awake () {
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad(gameObject);
	}

	public void PlayNote(int noteIndex)
	{
		if (noteIndex > NoteClips.Length - 1) return;
		EffectSource.clip = NoteClips[noteIndex];
		EffectSource.Play();
	}
	
	public void PlayErrorSound()
	{
		EffectSource.clip = ErrorSound;
		EffectSource.Play();
	}
}
