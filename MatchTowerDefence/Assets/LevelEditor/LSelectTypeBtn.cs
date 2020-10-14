using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LSelectTypeBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            //select specific type of tower
            LEditorManager.GetInstance().usingelement = thisType;
            img.sprite = GetComponent<Image>().sprite;
            canvas.SetActive(false);
        }

        public LEditorManager.ElementType thisType;
        public Image img;
        public GameObject canvas;
       
    }
}