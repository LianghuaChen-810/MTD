using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LElementSetBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {

        }
    }
}