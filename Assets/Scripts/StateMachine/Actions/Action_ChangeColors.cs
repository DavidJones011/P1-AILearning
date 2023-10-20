using System;
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

        public override EActionResult ExecuteAction(AIBrain brain)
        {
            MeshRenderer renderer = brain.gameObject.GetComponent<MeshRenderer>();
            if (renderer == null)
                return EActionResult.FAILURE;

            brain.GetBlackboard().SetFloatValue("ColorTimer", 0.0f);
            return EActionResult.RUNNING;
        }

        public override void ExitAction(AIBrain brain, bool success)
        {
            Color otherColor = Color.white;
            brain.GetBlackboard().GetColorValue("Color", ref otherColor);
            MeshRenderer renderer = brain.gameObject.GetComponent<MeshRenderer>();
            renderer.material.color = otherColor;
        }

        public override EActionResult TickAction(float deltaTime, AIBrain brain)
        {
            float timer = 0.0f;
            brain.GetBlackboard().GetFloatValue("ColorTimer", ref timer);
            timer += deltaTime * _frequency;
            brain.GetBlackboard().SetFloatValue("ColorTimer", timer);

            float t = 1.0f - (Mathf.Cos(timer) * 0.5f + 0.5f);

            Color otherColor = Color.white;
            brain.GetBlackboard().GetColorValue("Color", ref otherColor);
            MeshRenderer renderer = brain.gameObject.GetComponent<MeshRenderer>();
            renderer.material.color = Color.Lerp(otherColor, _color, t);

            return EActionResult.RUNNING;
        }
    }
}
