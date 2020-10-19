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

                //set moves
                if (LEditorManager.GetInstance().editingwave < LEditorManager.GetInstance().moves.Count)
                {
                    movesipf.text = LEditorManager.GetInstance().moves[LEditorManager.GetInstance().editingwave].ToString();
                }
                else
                {
                    movesipf.text = "1";
                }
            }
        }

        public Text stxt;
        public LAddMonsterBtn addbtn;
        public InputField movesipf;
    }
}