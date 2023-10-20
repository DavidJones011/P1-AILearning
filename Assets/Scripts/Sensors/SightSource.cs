using AILearning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSource : MonoBehaviour, SightSourceInterface
{
    [SerializeField]
    private float _halfHeight = 1.0f;

    [SerializeField]
    private float _radius = 0.25f;

    [SerializeField]
    private bool _losCheck = false;

    public Bounds GetSightSourceBounds()
    {
        var bounds = new Bounds();
        bounds.center = gameObject.transform.position;
        bounds.extents = new Vector3(_radius, _halfHeight, _radius);
        return bounds;
    }

    public bool CanBeSeen(in Vector3 observerPosition, in GameObject observerObject)
    {
        // in case the source and the observer are the same gameobject
        // we want to ignore oneself
        if (gameObject == observerObject)
            return false;

        // We can do additional checks to check for early outs!
        //  - Is the character in the dark?
        //  - Are they on the same team?

        if (_losCheck)
        {
            RaycastHit result;
            Vector3 direction = (gameObject.transform.position - observerPosition);
            float distance = direction.magnitude;
            bool hit = Physics.Raycast(observerPosition, direction.normalized, out result, distance);

            if (hit && result.collider.GetComponent<Collider>().gameObject != gameObject)
            {
                return false;
            }

        }

        return true;
    }

    public void Start()
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

        sight.RegisterSource(this);
    }
}
