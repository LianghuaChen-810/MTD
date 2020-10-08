using UnityEngine;

namespace GameCore.System
{
    public class LevelManager
    {
        public static LevelManager instance = null;

        public static LevelManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LevelManager();
                }
                return instance;
            }
        }


    }
}
