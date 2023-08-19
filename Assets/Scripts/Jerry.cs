using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AILearning;

public class Jerry : MonoBehaviour
{
    [SerializeField]
    float _soundDistance = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            HearSense.ReportNoise(transform.position, _soundDistance, "fart");
        }
    }
}
