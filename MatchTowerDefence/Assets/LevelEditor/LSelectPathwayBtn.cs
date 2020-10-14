using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LSelectPathwayBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            LEditorManager.GetInstance().usingelement = LEditorManager.ElementType.Pathway;
            highlightblock.position = transform.position;
        }



        public RectTransform highlightblock;
    }
}