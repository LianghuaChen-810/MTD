using GameCore.System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MatchTowerDefence.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelSelectButton : MonoBehaviour, ISelectHandler
    {
        protected Button button;
        protected MouseScroll mouseScroll;
        protected LevelItem item;

        public TMP_Text titleDisplay;
        public TMP_Text description;
        public Sprite starsAchieved;
        public Image[] stars;

        public void ButtonClicked()
        {
            ChangeScenes();
        }

        public void Initialize(LevelItem _item, MouseScroll _mouseScroll)
        {
            LazyLoad();
            if(titleDisplay == null) { return; }
            item = _item;
            titleDisplay.text = item.name;
            description.text = item.description;
            HasPlayedState();
            mouseScroll = _mouseScroll;
        }

        private void HasPlayedState()
        {
            if(LevelManager.instance == null) { return; }

            int starsForLevel = LevelManager.instance.GetStarsForLevel(item.id);
            for(int i = 0; i < starsForLevel;++i)
            {
                stars[i].sprite = starsAchieved;
            }
        }

        protected void LazyLoad()
        {
           if(button == null) { button = GetComponent<Button>(); }
        }

        protected void ChangeScenes()
        {
            SceneManager.LoadScene(item.sceneName);
        }

        protected void OnDestroy()
        {
            if(button != null) { button.onClick.RemoveAllListeners(); }    
        }

        public void OnSelect(BaseEventData eventData)
        {
            mouseScroll.SelectChild(this);
        }
    }
}