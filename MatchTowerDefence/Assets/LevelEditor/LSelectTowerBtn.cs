using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LSelectTowerBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            highlightblock.position = transform.position;
            // choose level of the tower
        }


        void Update()
        {

        }

        public LEditorManager.ElementType towertype;
        public GameObject[] prefabs;

        public RectTransform highlightblock;
    }
}