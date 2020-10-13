using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MatchTowerDefence.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager instance = null;

        private const string saveFile = "SaveFile";

        [SerializeField] private AudioMixer gameAudioMixer;
        [SerializeField] private string masterVolume;
        [SerializeField] private string musicVolume;
        [SerializeField] private string sfxVolume;

        private JSONSave<SaveGameDataStore> dataSaver;
        public SaveGameDataStore saveData;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = GetComponent<SaveManager>();
            }
            else
            {
                Destroy(gameObject);
            }

            LoadData();
        }

        private void Start()
        {
            SetVolumes(saveData.masterVolume, saveData.musicVolume, saveData.musicVolume, false);
        }

        public void GetVolumes(out float master, out float music, out float sfx)
        {
            master = saveData.masterVolume;
            music = saveData.musicVolume;
            sfx = saveData.sfxVolume;
        }


        public void SetVolumes(float master, float music, float sfx, bool save)
        {
            if(gameAudioMixer == null) { return; }

            if(masterVolume != null)
            {
                gameAudioMixer.SetFloat(masterVolume, LogarithmicTransform(Mathf.Clamp01(master)));
            }
            
            if(musicVolume != null)
            {
                gameAudioMixer.SetFloat(musicVolume, LogarithmicTransform(Mathf.Clamp01(music)));
            } 
            
            if(sfxVolume != null)
            {
                gameAudioMixer.SetFloat(sfxVolume, LogarithmicTransform(Mathf.Clamp01(sfx)));
            }

            if(save)
            {
                saveData.masterVolume = master;
                saveData.musicVolume = music;
                saveData.sfxVolume = sfx;
                SaveData();
            }
        }

        private void SaveData()
        {
            dataSaver.Save(saveData);
        } 
        
        private void LoadData()
        {
            #if UNITY_EDITOR
                dataSaver = new JSONSave<SaveGameDataStore>(saveFile);
            #else
                dataSaver = new EncryptedJSON<SaveGameDataStore>(saveFile);
            #endif

            try
			{
				if (!dataSaver.Load(out saveData))
				{
                    saveData = new SaveGameDataStore();
                    SaveData();
				}
			}
			catch (Exception)
			{
				Debug.Log("Failed to load data");
				saveData = new SaveGameDataStore();
				SaveData();
			}
        }

        private static float LogarithmicTransform(float volume)
        {
            volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
            return volume - 80;
        }
    }
}
