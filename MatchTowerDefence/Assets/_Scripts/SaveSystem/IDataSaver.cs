namespace MatchTowerDefence.SaveSystem
{
    public interface IDataSaver<Data> where Data : IData
    {
        void Save(Data data);
        bool Load(out Data data);
        void Clear();

    }
}