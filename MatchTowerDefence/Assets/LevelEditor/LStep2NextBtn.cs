using System.Collections;
using System.Collections.Generic;
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
            step2.SetActive(false);
            step3.SetActive(true);
            LEditorManager.step = 3;
        }
        public GameObject step2;
        public GameObject step3;
    }
}