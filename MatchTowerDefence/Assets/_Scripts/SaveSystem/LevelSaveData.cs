using System;
using System.Collections.Generic;

namespace MatchTowerDefence.SaveSystem
{
    [System.Serializable]
    public class LevelSaveData
    {
        public string levelId;
        public int numberOfStars;

        public LevelSaveData(string _levelId, int _noOfStars)
        {
            levelId = _levelId;
            numberOfStars = _noOfStars;
        }
    }
}
