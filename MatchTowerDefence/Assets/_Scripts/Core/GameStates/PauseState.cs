using GameCore.System;
using UnityEngine;

namespace GameCore.GameStates
{
    public class PauseState : GameCore.System.State
    {
        GameStateController gameStateController;

        public PauseState(GameStateController owner) : base(owner)
        {
            Time.timeScale = 0;
            gameStateController = owner;
            GUIManager.instance.pauseMenu.SetActive(true);
            Debug.Log("Game is paused");
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
            owner.State = new PlayingState(gameStateController);
        }
    }
}