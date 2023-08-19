using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AILearning
{
    public interface HearingListenerInterface
    {
        public void UpdateListener(NoiseEvent noiseEvent);
    }
}
