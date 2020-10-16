using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
namespace LevelEditor
{
    public class LStep3TestBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void Update()
        {
            if (cantest)
            {
                txt.text = "Test";
            }
        }

        private void OnClick()
        {
            if (cantest)
            {
                cantest = false;
                SceneManager.LoadScene("LevelTest");
            }
        }

        public static bool cantest = false;

        public Text txt;
    }
}