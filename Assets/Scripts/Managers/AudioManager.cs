using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

	[SerializeField]
	private SO_AudioClips audioClips;

	private AudioSource audioSource;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		audioSource = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Play clip by ID. Allowing for audio clips to be switched out easily outside of code using scriptable object.
	/// </summary>
	/// <param name="clipID">id of the clip</param>
	/// <param name="volume"></param>
	public void PlayClip(string clipID, float volume)
	{
		AudioClip clip;
		if (audioClips.GetClipFromID(clipID, out clip))
			audioSource.PlayOneShot(clip, volume);
	}


}
