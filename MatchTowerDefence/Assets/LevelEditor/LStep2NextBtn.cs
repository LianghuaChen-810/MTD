using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

using UnityEngine.UI;
namespace LevelEditor
{
    public class LStep2NextBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            // if (LEditorManager.GetInstance().routes.Count > 0 && LAddMonsterBtn.options.Count > 0)
           if (LEditorManager.GetInstance().routes.Count > 0 && LEditorManager.GetInstance().waves[0].listnum > 0)
            {
                /*
                //save monsters
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

                LEditorManager.GetInstance().monsters = md;
                */
                step2.SetActive(false);
                step3.SetActive(true);
                LEditorManager.step = 3;
            }
        }
        public GameObject step2;
        public GameObject step3;
    }
}