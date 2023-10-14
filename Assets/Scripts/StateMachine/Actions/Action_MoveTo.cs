using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AILearning
{
    // Simple action where the npc will just spin in circles for a duration of time.
    [CreateAssetMenu(fileName = "Action_MoveTo", menuName = "AI/StateMachine/Action/Action_MoveTo", order = 1)]
    public class Action_MoveTo : Action
    {
        [SerializeField]
        float _speed = 5.0f;

        [SerializeField]
        float _desiredGoalDistance = 0.2f;

        [SerializeField]
        string _blackboardKeyName = "MoveLocation";

        Vector3 _targetLocation = Vector3.zero;

        public override EActionResult ExecuteAction(AIBrain brain)
        {
            if(!brain.GetBlackboard().GetVectorValue(_blackboardKeyName, ref _targetLocation)) // failed to find bb key
                return EActionResult.FAILURE;

            Vector3 location = brain.gameObject.transform.position;
            if(Vector3.SqrMagnitude(location - _targetLocation) <= (_desiredGoalDistance * _desiredGoalDistance))
            {
                // already at the goal
                return EActionResult.SUCCESS;
            }

            if(!brain.GetComponent<NavMeshAgent>().SetDestination(_targetLocation)) // couldn't find path
                return EActionResult.FAILURE;

            return EActionResult.RUNNING;
        }

        public override void ExitAction(AIBrain brain, bool success)
        {
            // nothing
        }

        public override EActionResult TickAction(float deltaTime, AIBrain brain)
        {
            // success when we are close enough
            Vector3 location = brain.gameObject.transform.position;
            if (Vector3.SqrMagnitude(location - _targetLocation) <= (_desiredGoalDistance * _desiredGoalDistance))
            {
                // already at the goal
                return EActionResult.SUCCESS;
            }

            return EActionResult.RUNNING;
        }
    }
}
