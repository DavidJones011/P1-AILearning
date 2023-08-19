using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bob : MonoBehaviour
{
    TextMesh _mesh = null;
    const string _defaultText = "Bob's Thoughts";

    private IEnumerator coroutine;

    bool _hasTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = gameObject.GetComponentInChildren<TextMesh>();

        SightConeSensor sight = gameObject.GetComponent<SightConeSensor>();
        sight.onFoundTargets += OnGainedSightOfTargetEvent;
        sight.onLostTargets += OnLostSightOfTargetEvent;

        HearingSensor hearing = gameObject.GetComponent<HearingSensor>();
        hearing.onHeardSomething += OnHeardSomethingEvent;

        coroutine = ResetText();
    }
    private void OnDestroy()
    {
        SightConeSensor sight = gameObject.GetComponent<SightConeSensor>();
        sight.onFoundTargets -= OnGainedSightOfTargetEvent;
        sight.onLostTargets -= OnLostSightOfTargetEvent;

        HearingSensor hearing = gameObject.GetComponent<HearingSensor>();
        hearing.onHeardSomething -= OnHeardSomethingEvent;
    }

    void OnHeardSomethingEvent()
    {
        _mesh.text = "I heard that!";
        _mesh.color = Color.red;

        StartCoroutine(ResetText());
    }

    void OnGainedSightOfTargetEvent()
    {
        _hasTarget = true;
        _mesh.text = "I see you!";
        _mesh.color = Color.red;
    }

    void OnLostSightOfTargetEvent()
    {
        _hasTarget = false;
        _mesh.text = "Where did they go?";
        _mesh.color = Color.magenta;

        StartCoroutine(ResetText());
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(2.0f);

        if(!_hasTarget)
        {
            _mesh.text = _defaultText;
            _mesh.color = Color.white;
        }
    }
}
