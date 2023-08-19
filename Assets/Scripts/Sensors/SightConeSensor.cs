using System;
using System.Collections.Generic;
using AILearning;
using UnityEngine;

/// \brief This component represents the vision cone of an AI character. <summary>
///        The component keeps track of all its visible targets.
public class SightConeSensor : MonoBehaviour, SightListnerInterface
{
    [SerializeField]
    private float _angle = 30.0f;

    [SerializeField]
    private float _distance = 10.0f;

    [SerializeField]
    private Vector3 _offset = new Vector3(0.0f, 1.0f, 0.0f);

    [SerializeField]
    private bool _debugDraw = false;

    List<SightSourceInterface> _lastSighted = new List<SightSourceInterface>();

    // Delegate called after finding targets wheren there were none.
    public delegate void OnFoundTarget();
    public OnFoundTarget onFoundTargets;

    // Delegate called after losing all targets.
    public delegate void OnLostTargets();
    public OnLostTargets onLostTargets;

    // Delegate called after target count has changed.
    public delegate void OnChangedTargets();
    public OnChangedTargets onChangedTargets;

    // Start is called before the first frame update
    void Start()
    {
        AISenseID id = SightSense.ID;
        if (!id.IsValid())
        {
            Debug.LogWarning("Sight ID was invalid! It may have not been registered on time.");
        }

        SightSense sight = AISensorManager.Instance.GetSense(id) as SightSense;
        if (sight == null)
        {
            Debug.LogWarning("AI sense wasn't of type Sight or id pointed to null");
        }

        sight.RegisterListener(this);
    }

    void OnDestroyed()
    {
        AISenseID id = SightSense.ID;
        if (!id.IsValid()) return;

        SightSense sight = AISensorManager.Instance.GetSense(id) as SightSense;
        if (sight == null) return;

        sight.UnregisterListener(this);
    }

    // Performs sight checks
    public void UpdateListener(in List<SightSourceInterface> sources)
    {
        Queue<SightSourceInterface> sightedSources = new Queue<SightSourceInterface>();
        Vector3 headLocation = gameObject.transform.position + _offset;

        foreach (var source in sources)
        {
            Bounds bounds = source.GetSightSourceBounds();
            Vector3 sourceDirection = (bounds.center - headLocation);

            // doing a simple cone overlap check
            {
                float distance = sourceDirection.sqrMagnitude;
                distance -= bounds.extents.sqrMagnitude;
                if (distance > (_distance * _distance))
                {
                    DrawDebugLine(headLocation, bounds.center, false);
                    continue;
                }

                float threshold = (float)Math.Cos(_angle);
                sourceDirection.Normalize();

                float dot = Vector3.Dot(gameObject.transform.forward, sourceDirection);
                if (dot < (1.0f - threshold))
                {
                    DrawDebugLine(headLocation, bounds.center, false);
                    continue;
                }
            }

            // check if the source says if it can be seen
            if(!source.CanBeSeen(headLocation, gameObject))
            {
                DrawDebugLine(headLocation, bounds.center, false);
                continue;
            }

            DrawDebugLine(headLocation, bounds.center, true);
            sightedSources.Enqueue(source);
        }

        // now we check for changes (events should be more meaningful)
        // TODO: create more events for finding sources
        {
            if (sightedSources.Count != _lastSighted.Count)
            {
                if(sightedSources.Count == 0)
                {
                    onLostTargets?.Invoke();
                }
                else if(_lastSighted.Count == 0)
                {
                    onFoundTargets?.Invoke();
                }
                else
                {
                    onChangedTargets?.Invoke();
                }
            }
            _lastSighted = new List<SightSourceInterface>(sightedSources);
        }

        if (_debugDraw)
        {
            DrawCone();
            DrawCircle();
        }
    }

    /// \brief Draws a simple cone shape using debug lines.
    private void DrawCone()
    {
        Vector3 headLocation = gameObject.transform.position + _offset;
        Vector3 direction = gameObject.transform.forward;

        Vector3 startDir = Quaternion.AngleAxis(_angle, Vector3.up) * direction * _distance;

        int iterations = 8;
        float step = 360.0f / iterations;
        for(float angle = 0.0f; angle < 360.0f; angle += step)
        {
            Vector3 rotatedDir = Quaternion.AngleAxis(angle, direction) * startDir;
            Debug.DrawLine(headLocation, headLocation + rotatedDir, Color.yellow);
        }
    }

    /// \brief Draws a simple circle shape
    private void DrawCircle()
    {
        Vector3 center = gameObject.transform.position + _offset;
        Vector3 direction = gameObject.transform.forward * _distance;

        int iterations = 16;
        float step = 360.0f / iterations;
        Vector3 lastPosition = center + direction;
        for (float angle = 0.0f; angle < 360.0f; angle += step)
        {
            Vector3 newPosition = center + Quaternion.AngleAxis(angle, Vector3.up) * direction;
            Debug.DrawLine(lastPosition, newPosition, Color.magenta);
            lastPosition = newPosition;
        }
        Debug.DrawLine(lastPosition, center + direction, Color.magenta);
    }

    private void DrawDebugLine(Vector3 from, Vector3 to, bool visible)
    {
        if (!_debugDraw) 
            return;

        Debug.DrawLine(from, to, visible ? Color.green : Color.red);
    }
}
