using GameCore.System;
using MatchTowerDefence.Level;
using MatchTowerDefence.Managers;
using MatchTowerDefence.SaveSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MatchTowerDefence.UI
{
    public class LevelSelectScreen : MonoBehaviour
    {
        [SerializeField] private LevelSelectButton selectionPrefab = null;
        [SerializeField] private LayoutGroup layout = null;
        [SerializeField] private Transform rightBuffer = null;
        [SerializeField] private MouseScroll mouseScroll = null;
        [SerializeField] private Button backButton = null;
        [SerializeField] private LevelInfoPanel levelInfoPanel = null;
        [SerializeField] private TMP_Text totalStars = null;

        protected LevelList levelList;
        protected List<Button> buttons = new List<Button>();

        // Start is called before the first frame update
        private void Start()
        {
            if(LevelManager.instance == null) { return; }

            levelList = LevelManager.instance.levelList;
            if (layout == null || selectionPrefab == null || levelList == null)
            {
                return;
            }

            int amount = levelList.Count;
            for (int i = 0; i < amount; ++i)
            {
                LevelSelectButton button = CreateButton(levelList[i]);
                button.transform.SetParent(layout.transform);
                button.transform.localScale = Vector3.one;
                buttons.Add(button.GetComponent<Button>());
            }
            if (rightBuffer != null)
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

            if(SaveManager.instance.saveData != null || SaveManager.instance.saveData.totalStars != 0)
            {
                totalStars.text = SaveManager.instance.saveData.totalStars + "/15";
            }
        }

        protected LevelSelectButton CreateButton(LevelItem item)
        {
            LevelSelectButton button = Instantiate(selectionPrefab);
            button.Initialize(item, mouseScroll, levelInfoPanel);
            GUIManager.instance.levelButtons.Add(button);
            return button;
        }

        private void SetUpNavigation(Selectable selectable, Selectable left, Selectable right)
        {
            Navigation navigation = selectable.navigation;
            navigation.selectOnLeft = left;
            navigation.selectOnRight = right;
            selectable.navigation = navigation;
        }

        public void UpdateTotalStars()
        {
            if (SaveManager.instance.saveData != null || SaveManager.instance.saveData.totalStars != 0)
            {
                totalStars.text = SaveManager.instance.saveData.totalStars + "/15";
            }
        }
    }
}
