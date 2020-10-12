using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

	public enum AudioClip { TowerAttack, Match };

	private AudioSource[] audioSource;

	// Use this for initialization
	private void Start()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = GetComponent<SFXManager>();
		}
		else
		{
			Destroy(gameObject);
		}
		audioSource = GetComponents<AudioSource>();
	}

	public void PlaySFX(AudioClip audioClip)
	{
		audioSource[(int)audioClip].Play();
	}
}
