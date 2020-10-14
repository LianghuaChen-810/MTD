using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LStep2BackBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            step1.SetActive(true);
            step2.SetActive(false);
            LEditorManager.step = 1;
        }
        public GameObject step1;
        public GameObject step2;
    }
}