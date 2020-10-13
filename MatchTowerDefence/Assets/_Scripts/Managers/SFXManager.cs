using System;
using UnityEngine;

namespace MatchTowerDefence.Managers
{
	public class SFXManager : MonoBehaviour
	{
		public static SFXManager instance;

		public enum AudioClip { TowerAttack, Match };

		private AudioSource[] audioSource;

		// Use this for initialization
		public void Awake()
		{
			DontDestroyOnLoad(gameObject);
			if (instance == null)
			{
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
}
