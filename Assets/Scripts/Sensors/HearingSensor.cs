using AILearning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

public class HearingSensor : MonoBehaviour, HearingListenerInterface
{
    [SerializeField]
    float _maxDistance = 20.0f;

    [SerializeField]
    bool _usePathFinding = false;

    [SerializeField]
    bool _losCheck = false;

    [SerializeField]
    bool _debugDraw = false;

    public delegate void OnHeardSomething();
    public OnHeardSomething onHeardSomething;

    // Start is called before the first frame update
    void Start()
    {
        AISenseID id = HearSense.ID;
        if (!id.IsValid())
        {
            Debug.LogWarning("HearSense ID was invalid! It may have not been registered on time.");
        }

        HearSense hearing = AISensorManager.Instance.GetSense(id) as HearSense;
        if (hearing == null)
        {
            Debug.LogWarning("AI sense wasn't of type HearSense or id pointed to null");
        }

        hearing.RegisterListener(this);
    }

    void OnDestroyed()
    {
        AISenseID id = HearSense.ID;
        if (!id.IsValid()) return;

        HearSense hearing = AISensorManager.Instance.GetSense(id) as HearSense;
        if (hearing == null) return;

        hearing.UnregisterListener(this);
    }

    public void UpdateListener(NoiseEvent noiseEvent)
    {
        float distSquared = noiseEvent.distance * noiseEvent.distance;
        Vector3 direction = noiseEvent.location - transform.position;

        bool success = false;

        // use pathfinding or euclidean distance check
        if (_usePathFinding)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, noiseEvent.location, -1, path))
            {
                // get the total distance of the path
                {
                    float totalDistance = 0.0f;
                    Vector3 lastPoint = transform.position;
                    foreach (var point in path.corners)
                    {
                        totalDistance += Vector3.Distance(point, lastPoint);
                        lastPoint = point;
                    }

                    if (totalDistance <= _maxDistance)
                        success = true;
                }

                // draw the path
                if(_debugDraw)
                {
                    Color color = success ? Color.green : Color.red;
                    Vector3 lastPoint = transform.position;
                    foreach (var point in path.corners)
                    {
                        Debug.DrawLine(lastPoint, point, color, 5.0f);
                        lastPoint = point;
                    }
                }
            }
        }
        else if (direction.sqrMagnitude <= distSquared)
        {
            success = true;

            // do a LOS check
            if (_losCheck)
            {
                Vector3 losDirection = gameObject.transform.position - noiseEvent.location;
                float distance = direction.magnitude;

                int mask = ~(1 << 3); // should try not to depend on hard coding the mask
                bool hit = Physics.Raycast(noiseEvent.location, losDirection, distance, mask);
                if (hit)
                    success = false;
            }

            if(_debugDraw)
            {
                Color color = success ? Color.green : Color.red;
                Debug.DrawLine(transform.position, noiseEvent.location, color, 5.0f);
            }
        }
        else
        {
            Debug.DrawLine(transform.position, noiseEvent.location, Color.red, 5.0f);
        }

        if(success)
        {
            onHeardSomething?.Invoke(); // heard something! broadcast it to anyone who cares.
        }
    }
}
