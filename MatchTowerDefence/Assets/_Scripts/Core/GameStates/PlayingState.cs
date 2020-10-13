using GameCore.System;
using UnityEngine;

namespace GameCore.GameStates
{
    public class PlayingState : State
    {
        GameStateController gameStateController;

        public PlayingState(GameStateController owner) : base(owner)
        {
            Time.timeScale = 1;
            gameStateController = owner;


            GUIManager.instance.LevelSceneInstance();
        }

        public override void OnUpdate()
        {

            if(LevelControl.phase == LevelPhase.FINISHED)
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
