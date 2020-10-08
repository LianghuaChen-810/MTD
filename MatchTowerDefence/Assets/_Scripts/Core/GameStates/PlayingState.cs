using GameCore.System;
using UnityEngine;

namespace GameCore.GameStates
{
    public class PlayingState : State
    {
        GameStateController gameStateController;

        private int enemyRemaining = 0;

        public PlayingState(GameStateController owner) : base(owner)
        {
            Time.timeScale = 1;
            gameStateController = owner;

            if (GUIManager.instance != null)
            {
                GUIManager.instance.InitiateGame();
            }
        }

        public override void OnUpdate()
        {
            enemyRemaining = BoardManager.instance.allEnemies.Count;

            if(enemyRemaining == 0 && GUIManager.instance.phaseTxt.text == "Defense Phase")
            {
                InitiateFinishedState();
            }

            GUIManager.instance.pauseButton.onClick.AddListener(() => InitiatePauseState());
            #if  UNITY_STANDALONE_WIN
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                InitiatePauseState();
            }
            #endif
        }
        
        private void InitiatePauseState()
        {
            owner.State = new PauseState(gameStateController);
        } 
        
        private void InitiateFinishedState()
        {
            owner.State = new LevelFinishedState(gameStateController);
        }
    }

}
