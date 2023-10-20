using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    public class Jimmy : MonoBehaviour
    {
        static string BB_NAME_MOVELOC = "MoveLocation";
        static string BB_NAME_CIRCENTER = "CircleCenter";

        //TODO: Instead of having blackboard keys update every frame for timers we should create a timer manager
        static string BB_NAME_CIRCLE_TIMER = "CircleTimer";
        static string BB_NAME_COLOR_TIMER = "ColorTimer";

        static string BB_NAME_COLOR = "Color";

        [SerializeField]
        GameObject target = null;

        Blackboard blackboard = null;

        // Start is called before the first frame update
        void Awake()
        {
            blackboard = GetComponent<Blackboard>();
            if(blackboard)
            {
                blackboard = GetComponent<Blackboard>();
                blackboard.RegisterBlackboardEntry(BB_NAME_MOVELOC, EntryType.VECTOR);
                blackboard.RegisterBlackboardEntry(BB_NAME_CIRCENTER, EntryType.VECTOR);
                blackboard.RegisterBlackboardEntry(BB_NAME_CIRCLE_TIMER, EntryType.FLOAT);
                blackboard.RegisterBlackboardEntry(BB_NAME_COLOR_TIMER, EntryType.FLOAT);
                blackboard.RegisterBlackboardEntry(BB_NAME_COLOR, EntryType.COLOR);

                blackboard.SetVectorValue(BB_NAME_MOVELOC, target ? target.transform.position : Vector3.zero);
                blackboard.SetColorValue(BB_NAME_COLOR, gameObject.GetComponent<MeshRenderer>().material.color);
            }
        }

        // Update is called once per frame
        void Update()
        {
            blackboard.SetVectorValue(BB_NAME_MOVELOC, target ? target.transform.position : Vector3.zero);        
        }
    }
}
