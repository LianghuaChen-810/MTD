using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LWaveSaveBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            int count = LAddMonsterBtn.options.Count;
            MonsterData md = ScriptableObject.CreateInstance<MonsterData>();

            md.monsterroutes = new int[count];
            md.monstertimes = new float[count];
            md.monstertypes = new int[count];
            for (int i = 0; i < count; i++)
            {
                md.listnum = count;
                md.monsterroutes[i] = LAddMonsterBtn.options[i].GetRouteNum();
                md.monstertimes[i] = LAddMonsterBtn.options[i].GetSpawnTime();
                md.monstertypes[i] = LAddMonsterBtn.options[i].GetMonsterType();
            }

            LEditorManager.GetInstance().waves[LEditorManager.GetInstance().editingwave] = md;
            
            //save  moves
            if (LEditorManager.GetInstance().editingwave < LEditorManager.GetInstance().moves.Count)
            {
                LEditorManager.GetInstance().moves[LEditorManager.GetInstance().editingwave] = Convert.ToInt32(movesipf.text);
            }
            else
            {
                LEditorManager.GetInstance().moves.Add(Convert.ToInt32(movesipf.text));
            }
        }


        public InputField movesipf;
    }
}