using System.Collections;
using System.Collections.Generic;
using AILearning;
using UnityEngine;

/// \brief Implementation of sight sense. All listeners and sources are registered in this sense.
public class SightSense : AISense
{
    private static AISenseID _id = AISenseID.InvalidID;
    public static AISenseID ID { get { return _id; } }

    List<SightListnerInterface> _listeners = new List<SightListnerInterface>();
    List<SightSourceInterface> _sources = new List<SightSourceInterface>();

    public void RegisterListener(SightListnerInterface listener) { _listeners.Add(listener); }
    public void UnregisterListener(SightListnerInterface listener) { _listeners.Remove(listener); }

    public void RegisterSource(SightSourceInterface source) { _sources.Add(source); }
    public void UnRegisterSource(SightSourceInterface source) { _sources.Remove(source); }

    public override void SetSenseID(AISenseID id) { _id = id; }
    public override AISenseID GetSenseID() { return _id; }

    public override string GetName() { return "Sight"; }

    public override bool WantsToTick() { return true; }

    public override void Tick(float deltaTime)
    {
        // we could chose to not tick every frame (for performance)
        // this is up to the designer/engineer
        foreach(var listener in _listeners)
        {
            listener.UpdateListener(_sources);
        }
    }

    public override void DebugDraw()
    {

    }
}
