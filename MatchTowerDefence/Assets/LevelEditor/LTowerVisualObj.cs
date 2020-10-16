using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class LTowerVisualObj : MonoBehaviour
    {
        public void SetSpriteByType(LEditorManager.ElementType type)
        {
            GetComponent<SpriteRenderer>().sprite = spritelib[(int)type];
        }

        public Sprite[] spritelib;
    }
}