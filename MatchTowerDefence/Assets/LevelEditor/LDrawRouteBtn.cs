using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LDrawRouteBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            LEditorManager.GetInstance().usingelement = LEditorManager.ElementType.Spawner;
            highlightblock.position = transform.position;
        }


        void Update()
        {

        }



        public RectTransform highlightblock;
    }
}