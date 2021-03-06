﻿using GameCore.System;
using MatchTowerDefence.Managers;
using UnityEngine;

namespace GameCore.GameStates
{
    public class PauseState : State
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
            if(!GUIManager.instance.pauseMenu.activeSelf) { InitiateNewState(); }   

            #if UNITY_STANDALONE_WIN
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