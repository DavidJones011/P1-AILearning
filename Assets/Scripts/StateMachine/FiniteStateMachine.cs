using AILearning;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AILearning
{
    // Base class for running AI decision making.
    // -- Call Run() when the decision maker should be running.
    // -- TODO: Expand this class
    [RequireComponent(typeof(AIBrain))]
    public abstract class AIDecisionMaker : MonoBehaviour
    {
        public bool IsRunning { get; private set; }
        AIBrain _brain = null;

        [SerializeField]
        bool _autoRunOnStart = true;

        public AIBrain GetBrain() { return _brain; }

        public void Start()
        {
            _brain = GetComponent<AIBrain>();
            if (_autoRunOnStart)
                Run();
        }

        public void Update()
        {
            if (IsRunning)
                Tick(Time.deltaTime, _brain);
        }

        // Runs decision maker.
        public virtual void Run()
        {
            IsRunning = true;
        }

        // Ticks the decision maker. Needs to be overriden in child class.
        protected abstract void Tick(float deltaTime, AIBrain brain);
    }

    // A simple FSM for ai characters.
    public class FiniteStateMachine : AIDecisionMaker
    {
        [SerializeField]
        State initialState = null;

        State currentState = null;
        State targetState = null;

        public override void Run()
        {
            base.Run();
            targetState = initialState;
        }

        protected override void Tick(float deltaTime, AIBrain brain)
        {
            EStateResult res = EStateResult.RUNNING;

            // checks to see if the target state is set (if so, then transition)
            if (targetState != null)
            {
                currentState = targetState;
                targetState = null;
                res = currentState.ExecuteState(brain);
            }

            // tick the state when applicable
            if (currentState && res == EStateResult.RUNNING)
            {
                res = currentState.TickState(deltaTime, brain);

                // if the state hasn't failed, check if we can transition
                if(res != EStateResult.FAILED)
                {
                    State newtarget = null;
                    if (currentState.CanTransition(brain, res, out newtarget))
                    {
                        targetState = newtarget;
                    }
                }
            }

            // if the state is no longer running, we want to exit the state
            if(currentState && res != EStateResult.RUNNING)
            {
                currentState.ExitState(brain, res == EStateResult.SUCCESS);
                currentState = null;
            }
        }
    }
}
