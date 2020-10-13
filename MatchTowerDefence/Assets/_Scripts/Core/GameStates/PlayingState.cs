using GameCore.System;
using UnityEngine;

namespace GameCore.GameStates
{
    public class PlayingState : State
    {
        GameStateController gameStateController;

        private int enemyRemaining = 9999;


        public PlayingState(GameStateController owner) : base(owner)
        {
            Time.timeScale = 1;
            gameStateController = owner;

            enemyRemaining = 9999;

            GUIManager.instance.LevelSceneInstance();
        }

        public override void OnUpdate()
        {
            if (BoardManager.instance != null) { enemyRemaining = BoardManager.instance.allEnemies.Count; }

            if(BoardManager.instance.defencePhase == true && enemyRemaining == 0)
            {
                InitiateFinishedState();
            }

            if (GUIManager.instance.pauseMenu.activeSelf) { InitiatePauseState(); }
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
