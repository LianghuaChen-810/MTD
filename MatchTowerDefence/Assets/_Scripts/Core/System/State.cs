namespace GameCore.system
{
    public abstract class State
    {
        protected Automaton owner;

        public State(Automaton _owner)
        {
            owner = _owner;
        }

        public abstract void OnUpdate();

        public abstract void OnFixedUpdate();
    }
}
