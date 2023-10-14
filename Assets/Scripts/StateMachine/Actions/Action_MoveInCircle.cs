using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    // Simple action where the npc will just spin in circles for a duration of time.
    [CreateAssetMenu(fileName = "Action_MoveInCircle", menuName = "AI/StateMachine/Action/Action_MoveInCircle", order = 1)]
    public class Action_MoveInCircle : Action
    {
        [SerializeField]
        float _duration = 5.0f;

        [SerializeField]
        float _speed = 5.0f;

        [SerializeField]
        float _radius = 1.5f;

        float _timer = 0.0f;
        Vector3 _center = Vector3.zero;

        public override EActionResult ExecuteAction(AIBrain brain)
        {
            _center = brain.gameObject.transform.position;
            _timer = _duration;

            if (_duration == 0.0f)
                return EActionResult.SUCCESS;
            else
                return EActionResult.RUNNING;
        }

        public override void ExitAction(AIBrain brain, bool success)
        {
            brain.gameObject.transform.position = _center;
        }

        public override EActionResult TickAction(float deltaTime, AIBrain brain)
        {
            Vector3 right = (Vector3.right * Mathf.Cos(_timer * _speed)) * _radius;
            Vector3 forward = (Vector3.forward * Mathf.Sin(_timer * _speed)) * _radius;
            brain.gameObject.transform.position = forward + right + _center;

            // if duration is negative, we run infinitely
            if (_duration > 0.0f)
                _timer = Mathf.Max(_timer - deltaTime, 0.0f);

            return _timer <= 0.0f && _duration > 0.0f ? EActionResult.SUCCESS : EActionResult.RUNNING;
        }
    }
}
