using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LWaveLastBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (LEditorManager.GetInstance().editingwave > 0)
            {
                LEditorManager.GetInstance().editingwave -= 1;
                stxt.text = "Save:" + LEditorManager.GetInstance().editingwave;

                //read and display and clear 
                addbtn.LoadData(LEditorManager.GetInstance().waves[LEditorManager.GetInstance().editingwave]);

            }
        }

        public Text stxt;
        public LAddMonsterBtn addbtn;
    }
}