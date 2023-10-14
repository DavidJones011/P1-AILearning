using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    // Base class for handling transitions between states
    // -- When the sm (state machine) needs to transition from one state to another, a transition is needed.
    // -- If all conditions are met in the transition then the sm will transition into the next state.
    [CreateAssetMenu(fileName = "Transition", menuName = "AI/StateMachine/Transition", order = 1)]
    public class Transition : ScriptableObject
    {
        [SerializeField]
        List<Condition> _conditions = new List<Condition>();

        [SerializeField]
        bool _transitionWithNoConditions = true;

        [SerializeField]
        bool _requireSuccessFromState = false;

        [SerializeField]
        State _target = null;

        // checks to see if all conditions have been met in the transition
        // -- if the conditions aren't met, we don't transition to target state
        public virtual bool MetConditions(AIBrain brain, EStateResult result)
        {
            if (_target == null)
                return false;

            if (_conditions.Count == 0)
                return _transitionWithNoConditions;

            if (_requireSuccessFromState && result != EStateResult.SUCCESS)
                return false;

            foreach (Condition c in _conditions)
            {
                if (!c.MetCondition(brain))
                    return false;
            }

            return true;
        }

        public State GetTargetState(){ return _target; }
    }
}
