using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    public enum EStateResult
    {
        RUNNING,
        SUCCESS,
        FAILED
    }

    // A state holds transitions and actions.
    // -- To define a behavior for AI you can have multiple actions that can be run concurrently.
    // -- (e.g. moving and shooting at the same time)
    [CreateAssetMenu(fileName = "State", menuName = "AI/StateMachine/State", order = 1)]
    public class State : ScriptableObject
    {
        [SerializeField]
        List<Action> actions = new List<Action>();

        [SerializeField]
        List<Transition> transitions = new List<Transition>();

        // returns if the state can currently transition.
        public bool CanTransition(AIBrain brain, EStateResult result, out State targetState)
        {
            foreach(Transition t in transitions)
            {
                if (t.MetConditions(brain, result))
                {
                    targetState = t.GetTargetState();
                    return true;
                }
            }
            targetState = null;
            return false;
        }

        // state entry point (entered state)
        public EStateResult ExecuteState(AIBrain brain)
        {
            EStateResult result = EStateResult.RUNNING;
            foreach (Action a in actions)
            {
                EActionResult actionResult = a.ExecuteAction(brain);
                if(actionResult != EActionResult.RUNNING)
                {
                    result = actionResult == EActionResult.SUCCESS ? EStateResult.SUCCESS : EStateResult.FAILED;
                    break;
                }
            }
            return result;
        }

        public EStateResult TickState(float deltaTime, AIBrain brain)
        {
            EStateResult result = EStateResult.RUNNING;
            foreach (Action a in actions)
            {
                EActionResult actionResult = a.TickAction(deltaTime, brain);
                if (actionResult != EActionResult.RUNNING)
                {
                    result = actionResult == EActionResult.SUCCESS ? EStateResult.SUCCESS : EStateResult.FAILED;
                    break;
                }
            }
            return result;
        }

        public void ExitState(AIBrain brain, bool success)
        {
            foreach (Action a in actions)
            {
                a.ExitAction(brain, success);
            }
        }
    }
}
