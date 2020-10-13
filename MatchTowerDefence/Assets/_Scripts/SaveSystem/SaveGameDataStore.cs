using System.Collections.Generic;
using UnityEngine;

namespace MatchTowerDefence.SaveSystem
{
    public class SaveGameDataStore : GameDataStoreBase
    {
        private const string saveFile = "SaveFile";

        public List<LevelSaveData> completedLevels = new List<LevelSaveData>();

        public override void PostLoad()
        {
            Debug.Log("Saving Game");
        }

        public override void PreSave()
        {
            Debug.Log("Loading Game");
        }

        public void CompleteLevel(string levelId, int starEarned)
        {
            foreach (LevelSaveData levelSaveData in completedLevels)
            {
                if (levelSaveData.levelId == levelId)
                {
                    levelSaveData.numberOfStars = Mathf.Max(levelSaveData.numberOfStars, starEarned);
                }
                completedLevels.Add(new LevelSaveData(levelId, starEarned));
            }
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