using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace LevelEditor
{
    public class LSelectElementBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            LEditorManager.GetInstance().usingelement = elementType;
            highlightblock.position = transform.position;
        }


        public LEditorManager.ElementType elementType;
        public RectTransform highlightblock;
    }
}
