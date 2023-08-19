using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    /// \brief Abstract class for AI senses.
    public abstract class AISense
    {
        public virtual void SetSenseID(AISenseID id) { }

        public virtual AISenseID GetSenseID() { return AISenseID.InvalidID; }

        public virtual bool WantsToTick() { return false; }

        public virtual string GetName() { return ""; }

        public virtual void Tick(float deltaTime) { }

        public virtual void DebugDraw() { }
    }
}
