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
            Debug.Log("Level Finished");
        }

        public override void OnUpdate()
        {
            GUIManager.instance.restartButton.onClick.AddListener(() => InitiatePlayingState());
        }

        private void InitiatePlayingState()
        {
            owner.State = new PlayingState(gameStateController);
        }
    }
}