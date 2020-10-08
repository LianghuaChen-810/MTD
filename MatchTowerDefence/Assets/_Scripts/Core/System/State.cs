namespace GameCore.System
{
    public abstract class State
    {
        protected StateMachine owner;

        public State(StateMachine _owner)
        {
            owner = _owner;
        }

        public abstract void OnUpdate();

        //public virtual void OnFixedUpdate() { }
    }
}
