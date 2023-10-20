using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    public enum EActionResult
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    // The base class for an action.
    // -- States can consist of multiple actions that can be run concurrently.
    // -- Actions may or maynot tick everyframe.
    public abstract class Action : ScriptableObject
    {       
        public abstract EActionResult ExecuteAction(AIBrain brain);

        public abstract EActionResult TickAction(float deltaTime, AIBrain brain);

        public abstract void ExitAction(AIBrain brain, bool success);
    }
}