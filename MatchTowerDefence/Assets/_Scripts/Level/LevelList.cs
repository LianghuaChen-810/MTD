using System;
using MatchTowerDefence.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MatchTowerDefence.Level
{
    [CreateAssetMenu(fileName = "LevelList", menuName = "Create Level List", order = 1)]
    public class LevelList : ScriptableObject, IList<LevelItem>, IDictionary<string, LevelItem>, ISerializationCallbackReceiver
    {
        public LevelItem[] levels;

        private IDictionary<string, LevelItem> levelDictionary;

        public int Count { get { return levels.Length; } }

        public LevelItem this[int index] { get { return levels[index]; }  }
        public LevelItem this[string key] { get { return levelDictionary[key]; }  }

        public bool IsReadOnly => true;

        public ICollection<string> Keys { get { return levelDictionary.Keys; } }

        public ICollection<LevelItem> Values => throw new System.NotImplementedException();

        public int IndexOf(LevelItem item)
        {
           if(item == null) { return -1; }

           for(int i = 0; i < levels.Length; ++i)
           {
               if(levels[i] == item) { return i; }
           }
           return -1;
        }

        public bool Contains(LevelItem item) { return IndexOf(item) >= 0; }

        public bool ContainsKey(string key) { return levelDictionary.ContainsKey(key); }

        public bool TryGetValue(string key, out LevelItem value) { return levelDictionary.TryGetValue(key, out value);  }

        public LevelItem GetLevelByScene(string scene)
        {
            for(int i = 0; i < levels.Length; ++i)
            {
                LevelItem levelItem = levels[i];
                if(levelItem != null && levelItem.sceneName == scene)
                {
                    return levelItem;
                }
            }
            return null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize() { levelDictionary = levels.ToDictionary(l => l.id); }

        ICollection<LevelItem> IDictionary<string, LevelItem>.Values { get { return levelDictionary.Values; } }

        LevelItem IList<LevelItem>.this[int index] 
        {
            get { return levels[index]; }
            set { throw new NotSupportedException("Level List is read only"); }
        }
         
        LevelItem IDictionary<string, LevelItem>.this[string key] 
        {
            get { return levelDictionary[key]; }
            set { throw new NotSupportedException("Level List is read only"); }
        } 

        void IList<LevelItem>.Insert(int index, LevelItem levelItem)
        {
            throw new NotSupportedException("Level List is read only");
        }
        
        void IList<LevelItem>.RemoveAt(int index)
        {
            throw new NotSupportedException("Level List is read only");
        }

        void ICollection<LevelItem>.Add(LevelItem levelItem)
        {
            throw new NotSupportedException("Level List is read only");
        }  
        
        void ICollection<KeyValuePair<string, LevelItem>>.Add(KeyValuePair<string, LevelItem> levelItem)
        {
            throw new NotSupportedException("Level List is read only");
        } 
        
        void ICollection<KeyValuePair<string, LevelItem>>.Clear()
        {
            throw new NotSupportedException("Level List is read only");
        }

        bool ICollection<KeyValuePair<string, LevelItem>>.Contains(KeyValuePair<string, LevelItem> levelItem)
        {
           return levelDictionary.Contains(levelItem);
        }
        
        void ICollection<KeyValuePair<string, LevelItem>>.CopyTo(KeyValuePair<string, LevelItem>[] array, int arrayIndex)
        {
           levelDictionary.CopyTo(array, arrayIndex);
        }

        void ICollection<LevelItem>.Clear()
        {
            throw new NotSupportedException("Level List is read only");
        }
        void ICollection<LevelItem>.CopyTo(LevelItem[] array, int arrayIndex)
        {
            levels.CopyTo(array, arrayIndex);
        }  
        
        bool ICollection<LevelItem>.Remove(LevelItem levelItem)
        {
            throw new NotSupportedException("Level List is read only");
        }

        public bool Contains(KeyValuePair<string, LevelItem> item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<LevelItem> GetEnumerator()
        {
            return ((IList<LevelItem>) levels).GetEnumerator();
        }
        IEnumerator<KeyValuePair<string, LevelItem>> IEnumerable<KeyValuePair<string, LevelItem>>.GetEnumerator()
        {
            return levelDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return levels.GetEnumerator();
        }

        void IDictionary<string, LevelItem>.Add(string key, LevelItem levelItem)
        {
            throw new NotSupportedException("Level List is read only");
        }

        bool ICollection<KeyValuePair<string, LevelItem>>.Remove(KeyValuePair<string, LevelItem> levelItem)
        {
            throw new NotSupportedException("Level List is read only");
        }

        bool IDictionary<string, LevelItem>.Remove(string key)
        {
            throw new NotSupportedException("Level List is read only");
        }
    }
}
