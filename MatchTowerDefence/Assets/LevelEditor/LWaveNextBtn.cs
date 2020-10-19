using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LWaveNextBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            LEditorManager.GetInstance().editingwave += 1;
            stxt.text = "Save:" + LEditorManager.GetInstance().editingwave;

            if (LEditorManager.GetInstance().waves.Count <= LEditorManager.GetInstance().editingwave)
            {
                // new data block
                LEditorManager.GetInstance().waves.Add(ScriptableObject.CreateInstance<MonsterData>());
            }

            //read and display and clear 
            addbtn.LoadData(LEditorManager.GetInstance().waves[LEditorManager.GetInstance().editingwave]);

        }

        public Text stxt;
        public LAddMonsterBtn addbtn;
    }
}