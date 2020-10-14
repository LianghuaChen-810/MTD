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
        public ScorePanel scorePanel = null;

        protected Button button = null;
        protected MouseScroll mouseScroll = null;
        protected LevelItem item = null;

        [SerializeField] private TMP_Text titleDisplay = null;
        [SerializeField] private TMP_Text description = null;
        [SerializeField] private Sprite starsAchieved = null;
        [SerializeField] private Image[] stars = null;
        [SerializeField] private LevelInfoPanel levelInfoPanel = null;

        public void ButtonClicked()
        {
            OpenLevelInfo();
        }

        public void Initialize(LevelItem _item, MouseScroll _mouseScroll, LevelInfoPanel _levelInfoPanel)
        {
            LazyLoad();
            if(titleDisplay == null) { return; }
            item = _item;
            titleDisplay.text = item.name;
            description.text = item.description;
            HasPlayedState();
            mouseScroll = _mouseScroll;
            levelInfoPanel = _levelInfoPanel;
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

        private void OpenLevelInfo()
        {
            mouseScroll.enabled = false;
            levelInfoPanel.gameObject.SetActive(true);
            levelInfoPanel.levelName.text = item.name;
            levelInfoPanel.playButton.onClick.AddListener(ChangeScenes);
        }


        protected void ChangeScenes()
        {
            mouseScroll.enabled = true;
            levelInfoPanel.gameObject.SetActive(false);
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