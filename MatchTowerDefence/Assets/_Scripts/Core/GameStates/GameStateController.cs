﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameStates
{
    public class GameStateController : GameCore.System.Automaton
    {
        private void Awake()
        {
            state = new PlayingState(this);
        }

        protected override void Update()
        {
            state.OnUpdate();
        }

        public void Resume()
        {
            state = new PlayingState(this);
        }

        public bool IsPaused()
        {
            return typeof(PauseState).IsInstanceOfType(state);
        }


    }
}
