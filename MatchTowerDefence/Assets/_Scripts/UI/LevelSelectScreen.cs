using GameCore.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchTowerDefence.UI
{
    public class LevelSelectScreen : MonoBehaviour
    {
        [SerializeField] private LevelSelectButton selectionPrefab;
        [SerializeField] private LayoutGroup layout;
        [SerializeField] private Transform rightBuffer;
        [SerializeField] private MouseScroll mouseScroll;
        [SerializeField] private Button backButton;

        //protected LevelList leveList;
        protected List<Button> buttons = new List<Button>();

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if(LevelManager.instance == null) { return;  }

            ///levelList = LevelManager.instance.levelList;
            if (layout == null || selectionPrefab == null  /*|| levelList == null*/ )
            {
                return;
            }

            //int amount = levelList.Count;
            //for(int i = 0; i < amount; ++i)
            //{
            //    LevelSelectButton button = CreateButton(leveList[i]);
            //    button.transform.SetParent(layout.transform);
            //    button.transform.localScale = Vector3.one;
            //    buttons.Add(button.GetComponent<Button>());
            //}
            if(rightBuffer != null)
            {
                rightBuffer.SetAsLastSibling();
            }

            for(int index = 1; index < buttons.Count - 1; ++index)
            {
                Button button = buttons[index];
                SetUpNavigation(buttons[0], backButton, buttons[1]);
                SetUpNavigation(buttons[buttons.Count - 1], buttons[buttons.Count - 2], null);

                mouseScroll.SetHasRightBuffer(rightBuffer != null);
            }

        }

        private void SetUpNavigation(Selectable selectable, Selectable left, Selectable right)
        {
            Navigation navigation = selectable.navigation;
            navigation.selectOnLeft = left;
            navigation.selectOnRight = right;
            selectable.navigation = navigation;
        }

        protected LevelSelectButton CreateButton(LevelItem item)
        {
            LevelSelectButton button = Instantiate(selectionPrefab);
            button.Initialize(item, mouseScroll);
            return button;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
