using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

namespace AILearning
{
    // Simple action where the npc will just spin in circles for a duration of time.
    [CreateAssetMenu(fileName = "Action_MoveInCircle", menuName = "AI/StateMachine/Action/Action_MoveInCircle", order = 1)]
    public class Action_MoveInCircle : Action
    {
        [SerializeField, Tooltip("Duration of spinning in a circle.")]
        float _duration = 5.0f;

        [SerializeField, Tooltip("Speed of spinning around in a circle.")]
        float _speed = 5.0f;

        [SerializeField, Tooltip("Radius of the circle to be spun around.")]
        float _radius = 1.5f;

        public override EActionResult ExecuteAction(AIBrain brain)
        {
            brain.GetBlackboard().SetVectorValue("CircleCenter", brain.transform.position);

            brain.GetBlackboard().SetFloatValue("CircleTimer", (_duration < 0.0f) ? 0.0f : _duration);

            if (_duration == 0.0f)
                return EActionResult.SUCCESS;
            else
                return EActionResult.RUNNING;
        }

        public override void ExitAction(AIBrain brain, bool success)
        {
            Vector3 location = Vector3.zero;
            brain.GetBlackboard().GetVectorValue("CircleCenter", ref location);
            brain.gameObject.transform.position = location;
        }

        public override EActionResult TickAction(float deltaTime, AIBrain brain)
        {
            Vector3 center = Vector3.zero;
            brain.GetBlackboard().GetVectorValue("CircleCenter", ref center);

            float timer = 0.0f;
            brain.GetBlackboard().GetFloatValue("CircleTimer", ref timer);

            Vector3 right = (Vector3.right * Mathf.Cos(timer * _speed)) * _radius;
            Vector3 forward = (Vector3.forward * Mathf.Sin(timer * _speed)) * _radius;
            brain.gameObject.transform.position = forward + right + center;

            // if duration is negative, we run infinitely
            if (_duration > 0.0f)
            {
                timer = Mathf.Max(timer - deltaTime, 0.0f);
                brain.GetBlackboard().SetFloatValue("CircleTimer", timer);
            }
            else
            {
                timer += deltaTime;
                brain.GetBlackboard().SetFloatValue("CircleTimer", timer);
            }

            return timer <= 0.0f && _duration > 0.0f ? EActionResult.SUCCESS : EActionResult.RUNNING;
        }
    }
}
