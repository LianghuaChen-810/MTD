using System.Collections.Generic;
using UnityEngine;
using static MatchTowerDefence.SaveSystem.LevelSaveData;

namespace MatchTowerDefence.SaveSystem
{
    public class SaveGameDataStore : GameDataStoreBase
    {
        private const string saveFile = "SaveFile";

        public int totalStars = 0;
        public List<LevelSaveData> completedLevels = new List<LevelSaveData>();

        public override void PostLoad()
        {
            Debug.Log("Loaded Game");
        }

        public override void PreSave()
        {
            Debug.Log("Saving Game");
        }

        public void CompleteLevel(string levelId, int starEarned)
        {
            foreach (LevelSaveData levelSaveData in completedLevels)
            {
                if (levelSaveData.levelId == levelId)
                {
                    if (starEarned > levelSaveData.numberOfStars)
                    {
                        totalStars += starEarned - levelSaveData.numberOfStars;
                    }
                    levelSaveData.numberOfStars = Mathf.Max(levelSaveData.numberOfStars, starEarned);
                    return;
                }
            }
            completedLevels.Add(new LevelSaveData(levelId, starEarned));
            totalStars += starEarned;
        }

        public bool IsLevelCompleted(string levelId)
        {
            foreach (LevelSaveData levelSaveData in completedLevels)
            {
                if (levelSaveData.levelId == levelId)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetNoOfStarsForLevel(string levelId)
        {
            foreach (LevelSaveData levelSaveData in completedLevels)
            {
                if (levelSaveData.levelId == levelId)
                {
                    return levelSaveData.numberOfStars;
                }
            }

            return 0;
        }
    }
}