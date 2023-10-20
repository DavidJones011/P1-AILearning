using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    Camera _cam = null;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Health health = hit.transform.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(20.0f);
                }
            }
        }
    }
}
