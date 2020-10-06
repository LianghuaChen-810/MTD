using GameCore.System;
using UnityEngine;

namespace GameCore.GameStates
{
    public class PauseState : GameCore.System.State
    {
        GameStateController gameStateController;
        State previousState;


        public PauseState(GameStateController owner) : base(owner)
        {
            Time.timeScale = 0;
            gameStateController = owner;



            Debug.Log("Game is paused");
        }

        public override void OnUpdate()
        {
            #if UNITY_ANRDOID
                 // Android code goes here
            #elif UNITY_IOS
                 // IOS Code goes here
            #endif
        }
    }
}