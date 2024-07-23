using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips", menuName = "ScriptableObjects/AudioClips", order = 1)]
public class SO_AudioClips : ScriptableObject
{
    public List<AudioClipAndID> clips = new List<AudioClipAndID>();

	public bool GetClipFromID(string ID, out AudioClip clip)
	{
		clip = null;
		foreach (AudioClipAndID clipAndID in clips)
		{
			if (clipAndID.ID == ID)
			{
				clip = clipAndID.clip;
				return true;
			}
		}
		Debug.LogError("Failed to find clip with id " +  ID);
		return false;
	}
}

[Serializable]
public struct AudioClipAndID
{
    public AudioClip clip;
    public string ID;
}
