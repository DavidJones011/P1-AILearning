using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AILearning
{
    // Simple action where the npc will just spin in circles for a duration of time.
    [CreateAssetMenu(fileName = "Action_ChangeColors", menuName = "AI/StateMachine/Action/Action_ChangeColors", order = 1)]
    public class Action_ChangeColors : Action
    {
        [SerializeField]
        Color _color = Color.white;

        [SerializeField]
        float _frequency = 5.0f;

        float _timer = 0.0f;
        Color _origColor = Color.white;

        public override EActionResult ExecuteAction(AIBrain brain)
        {
            MeshRenderer renderer = brain.gameObject.GetComponent<MeshRenderer>();
            if (renderer == null)
                return EActionResult.FAILURE;

            _timer = 0.0f;
            _origColor = renderer.material.color;
            return EActionResult.RUNNING;
        }

        public override void ExitAction(AIBrain brain, bool success)
        {
            MeshRenderer renderer = brain.gameObject.GetComponent<MeshRenderer>();
            renderer.material.color = _origColor;
        }

        public override EActionResult TickAction(float deltaTime, AIBrain brain)
        {
            _timer += deltaTime * _frequency;
            float t = 1.0f - (Mathf.Cos(_timer) * 0.5f + 0.5f);

            MeshRenderer renderer = brain.gameObject.GetComponent<MeshRenderer>();
            renderer.material.color = Color.Lerp(_origColor, _color, t);

            return EActionResult.RUNNING;
        }
    }
}
