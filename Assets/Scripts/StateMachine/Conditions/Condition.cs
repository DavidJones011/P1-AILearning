using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    // enum that can be used for comparing types in conditions
    public enum ComparisonType
    {
        GREATER_THAN,
        GREATER_THAN_OR_EQUAL,
        LESS_THAN,
        LESS_THAN_OR_EQUAL,
        EQUAL,
        NOT_EQUAL
    }

    // Base class for a transition condition.
    // -- A transition can contain multiple conditions.
    // -- Conditions dictates whether the sm can transition from one state to another.
    public abstract class Condition : ScriptableObject
    {
        public abstract bool MetCondition(AIBrain brain);
    }
}
