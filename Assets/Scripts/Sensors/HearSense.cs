using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AILearning;
using JetBrains.Annotations;
using System;

/// \brief Simple noise event structure.
public struct NoiseEvent
{
    public Vector3 location;
    public float distance;
    public string name;
}

/// \brief Implementation of hearing sense. All listeners and sources are registered in this sense.
public class HearSense : AISense
{
    private static AISenseID _id = AISenseID.InvalidID;
    List<HearingListenerInterface> _listeners = new List<HearingListenerInterface>();

    public static AISenseID ID { get { return _id; } }

    public override void SetSenseID(AISenseID id) { _id = id; }

    public void RegisterListener(HearingListenerInterface listener) { _listeners.Add(listener); }
    public void UnregisterListener(HearingListenerInterface listener) { _listeners.Remove(listener); }

    public override string GetName() { return "Hearing"; }

    /// \brief Notifies all listeners of noise
    public void BroadcastNoiseEvent(NoiseEvent noiseEvent)
    {
        foreach (var listener in _listeners)
        {
            listener.UpdateListener(noiseEvent);
        }
    }

    public static void ReportNoise(Vector3 location, float distance, string name = "")
    {
        NoiseEvent e;
        e.location = location;
        e.distance = distance;
        e.name = name;

        AISenseID id = HearSense.ID;
        if (!id.IsValid()) return;

        HearSense hearing = AISensorManager.Instance.GetSense(id) as HearSense;
        if (hearing == null) return;

        hearing.BroadcastNoiseEvent(e);
    }
}
