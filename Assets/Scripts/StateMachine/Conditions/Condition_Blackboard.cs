using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AILearning
{
    public abstract class Condition_Blackboard : Condition
    {
        [SerializeField]
        protected ComparisonType _type = ComparisonType.EQUAL;

        [SerializeField]
        protected string _key = "";

        [SerializeField]
        protected float _epsilon = 0.01f;

        public override bool MetCondition(AIBrain brain)
        {
            return MetCondition_Implementation(brain.GetBlackboard());
        }

        protected abstract bool MetCondition_Implementation(Blackboard blackboard);
    }
}
