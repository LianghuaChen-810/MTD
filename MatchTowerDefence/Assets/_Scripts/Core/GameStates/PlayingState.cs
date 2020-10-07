using GameCore.System;
using UnityEngine;

namespace GameCore.GameStates
{
    public class PlayingState : GameCore.System.State
    {
        GameStateController gameStateController;

        public PlayingState(GameStateController owner) : base(owner)
        {
            GUIManager.instance.pauseMenu.SetActive(false);
            Time.timeScale = 1;
            gameStateController = owner;

            GUIManager.instance.InitiateGame();
            Debug.Log("Game is playing");
        }

        public override void OnUpdate()
        {
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

      
    }

}
