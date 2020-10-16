using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LStep3BackBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            step2.SetActive(true);
            step3.SetActive(false);
            LEditorManager.step = 2;
        }
        public GameObject step2;
        public GameObject step3;
    }
}

