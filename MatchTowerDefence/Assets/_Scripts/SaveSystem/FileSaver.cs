using System.IO;
using UnityEngine;

namespace MatchTowerDefence.SaveSystem
{
    public abstract class FileSaver<Data> : IDataSaver<Data> where Data : IData
    {
        protected string fileName;

        protected FileSaver(string _fileName)
        {
            fileName = GetFileName(_fileName);
        }

        private string GetFileName(string _fileName)
        {
            return string.Format("{0}/{1}", Application.dataPath, _fileName);
        }

        protected virtual StreamWriter WriteStream()
        {
            return new StreamWriter(new FileStream(fileName, FileMode.Create));
        }  
        
        protected virtual StreamReader ReadStream()
        {
            return new StreamReader(new FileStream(fileName, FileMode.Open));
        }

        public void Clear()
        {
            File.Delete(fileName);
        }

        public abstract bool Load(out Data data);

        public abstract void Save(Data data);
    }
}