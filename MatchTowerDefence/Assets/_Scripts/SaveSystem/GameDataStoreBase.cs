namespace MatchTowerDefence.SaveSystem
{
    public abstract class GameDataStoreBase : IData
    {
        public float masterVolume = 1;
        public float musicVolume = 1;
        public float sfxVolume = 1;

        public abstract void PreSave();
        public abstract void PostLoad();
    }
}
