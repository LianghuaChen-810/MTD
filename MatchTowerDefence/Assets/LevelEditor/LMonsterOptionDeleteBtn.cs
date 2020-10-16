using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LMonsterOptionDeleteBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            LAddMonsterBtn.options.Remove(monster);
            LAddMonsterBtn.RefreshOptions();
            Destroy(monster.gameObject);
        }
        public LMonsterOption monster;
    }
}