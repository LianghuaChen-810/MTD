using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace LevelEditor
{
    public class LRouteOptionDrawBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (!candraw)
            {

                candraw = true;
                txt.text = "Stop";
                drawingroute = route;

            }
            else
            {
                candraw = false;
                txt.text = "Draw";

            }

        }

        public static bool candraw = false;
        public LRoute route;
        public static LRoute drawingroute;
        public Text txt;
    }
}

