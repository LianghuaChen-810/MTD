using System.IO;
using UnityEngine;

namespace MatchTowerDefence.SaveSystem
{
    public class JSONSave<Data> : FileSaver<Data> where Data : IData
    {
        public JSONSave(string _fileName) : base(_fileName) { }

        public override bool Load(out Data data)
        {
            if(!File.Exists(fileName))
            {
                data = default(Data);
                return false;
            }

            using(StreamReader reader = ReadStream()) { data = JsonUtility.FromJson<Data>(reader.ReadToEnd()); }

            return true;
        }

        public override void Save(Data data)
        {
            string json = JsonUtility.ToJson(data);
            Debug.Log(json);
            using(StreamWriter writer = WriteStream()) { writer.Write(json); }
        }
    }
}