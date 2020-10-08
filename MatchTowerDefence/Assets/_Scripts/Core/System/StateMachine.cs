using UnityEngine;

namespace GameCore.System
{
    //Automaton and state are used to implement the State Pattern just to ensure there is no need to have switch cases
    //This is done by wrapping each state in its own class

    public abstract class StateMachine : MonoBehaviour
    {
        protected State state = null;

        public State State { get { return state; } set { state = value; } }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (state != null)
            {
                state.OnUpdate();
            }
        }

        //protected virtual void FixedUpdate()
        //{
        //    if(state != null)
        //    {
        //        state.OnFixedUpdate();
        //    }
        //}
    }
}
