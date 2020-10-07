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
            #if UNITY_ANRDOID
                // Android code goes here
            #elif UNITY_IOS
                // IOS Code goes here
            #elif UNITY_STANDALONE_WIN
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                InitiateNewState();
            }
            #endif
        }
        
        private void InitiateNewState()
        {
            owner.State = new PauseState(gameStateController);
        }
    }

}
