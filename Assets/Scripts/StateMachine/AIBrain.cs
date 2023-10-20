using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AILearning
{
    [RequireComponent(typeof(Blackboard))]
    public class AIBrain : MonoBehaviour
    {
        Blackboard blackboard = null;

        public Blackboard GetBlackboard() { return blackboard; }

        private void Awake()
        {
            blackboard = GetComponent<Blackboard>();
        }
    }
}