using GameCore.System;

namespace GameCore.GameStates
{
    public class GameStateController : StateMachine
    {
        private void Awake()
        {
            state = new PlayingState(this);
        }

        protected override void Update()
        {
            state.OnUpdate();
        }

        public bool IsPaused()
        {
            return typeof(PauseState).IsInstanceOfType(state);
        }
    }
}
