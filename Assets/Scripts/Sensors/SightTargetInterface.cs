using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AILearning
{
    public interface SightSourceInterface
    {
        public Bounds GetSightSourceBounds(); 
        public bool CanBeSeen(in Vector3 receiverPosition, in GameObject receiverObject);
    }

    public interface SightListnerInterface
    {
        public void UpdateListener(in List<SightSourceInterface> sources);
    }
}
