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
        float _desiredGoalDistance = 0.2f;

        [SerializeField]
        string _blackboardKeyName = "MoveLocation";

        public override EActionResult ExecuteAction(AIBrain brain)
        {
            Vector3 targetLocation = Vector3.zero;
            if(!brain.GetBlackboard().GetVectorValue(_blackboardKeyName, ref targetLocation)) // failed to find bb key
                return EActionResult.FAILURE;

            Vector3 location = brain.gameObject.transform.position;
            if(Vector3.SqrMagnitude(location - targetLocation) <= (_desiredGoalDistance * _desiredGoalDistance))
            {
                // already at the goal
                return EActionResult.SUCCESS;
            }

            if(!brain.GetComponent<NavMeshAgent>().SetDestination(targetLocation)) // couldn't find path
                return EActionResult.FAILURE;

            return EActionResult.RUNNING;
        }

        public override void ExitAction(AIBrain brain, bool success)
        {
            // nothing
        }

        public override EActionResult TickAction(float deltaTime, AIBrain brain)
        {
            Vector3 targetLocation = Vector3.zero;
            brain.GetBlackboard().GetVectorValue(_blackboardKeyName, ref targetLocation);

            // TODO: Make an observer so that we don't recalc path every frame
            brain.gameObject.GetComponent<NavMeshAgent>().SetDestination(targetLocation);

            // success when we are close enough
            Vector3 location = brain.gameObject.transform.position;
            float distSquared = Vector3.SqrMagnitude(location - targetLocation);
            if (distSquared <= (_desiredGoalDistance * _desiredGoalDistance))
            {
                // already at the goal
                return EActionResult.SUCCESS;
            }

            return EActionResult.RUNNING;
        }
    }
}
