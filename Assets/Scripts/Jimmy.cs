using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AILearning
{
    public class Jimmy : MonoBehaviour
    {
        static string BB_NAME_NUMBER = "number";
        static string BB_NAME_MOVELOC = "MoveLocation";

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
                blackboard.RegisterBlackboardEntry(BB_NAME_NUMBER, EntryType.INT);
                blackboard.RegisterBlackboardEntry(BB_NAME_MOVELOC, EntryType.VECTOR);

                int value = -1;
                blackboard.GetIntValue(BB_NAME_NUMBER, ref value);
                blackboard.SetVectorValue(BB_NAME_MOVELOC, target ? target.transform.position : Vector3.zero);
                //print(value);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int value = -1;
                blackboard.SetIntValue(BB_NAME_NUMBER, 20);
                //blackboard.GetIntValue(BB_NAME_NUMBER, ref value);
                //print(value)

                blackboard.SetVectorValue(BB_NAME_MOVELOC, target ? target.transform.position : Vector3.zero);
            }
        }
    }
}
