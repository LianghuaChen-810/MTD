using GameCore.System;
using UnityEngine;

namespace GameCore.GameStates
{
    public class LevelFinishedState : State
    {
        GameStateController gameStateController;

        public LevelFinishedState(GameStateController owner) : base(owner)
        {
            Time.timeScale = 0;
            gameStateController = owner;
            GUIManager.instance.levelFinishedMenu.SetActive(true);
            GUIManager.instance.LevelIsFinished();

            Debug.Log("Level Finished");
        }

        public override void OnUpdate()
        {
            // Why add on every update instead in construction?
            GUIManager.instance.restartButton.onClick.AddListener(() => InitiatePlayingState());
        }

        private void InitiatePlayingState()
        {

            // First reload the scene
            GUIManager.instance.Play();
            owner.State = new PlayingState(gameStateController);

            // Why was this here :D 
           // GUIManager.instance.LevelIsFinished();

        }
    }
}