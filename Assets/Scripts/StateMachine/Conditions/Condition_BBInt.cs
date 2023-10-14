using AILearning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    [CreateAssetMenu(fileName = "Condition_BBInt", menuName = "AI/StateMachine/Condition/Condition_BlackboardInt", order = 1)]
    public class Condition_BBInt : Condition_Blackboard
    {
        [SerializeField]
        int _value = 0;

        protected override bool MetCondition_Implementation(Blackboard blackboard)
        {
            int bbvalue = 0;
            if (!blackboard.GetIntValue(_key, ref bbvalue))
                return false;

            switch (_type)
            {
                case ComparisonType.GREATER_THAN:
                    return bbvalue > _value;
                case ComparisonType.GREATER_THAN_OR_EQUAL:
                    return bbvalue >= _value;
                case ComparisonType.LESS_THAN:
                    return bbvalue < _value;
                case ComparisonType.LESS_THAN_OR_EQUAL:
                    return bbvalue <= _value;
                case ComparisonType.EQUAL:
                    return bbvalue == _value;
                case ComparisonType.NOT_EQUAL:
                    return bbvalue != _value;
                default:
                    return false;
            }
        }
    }

}