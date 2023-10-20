using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SODemo_CharacterData", menuName = "Demo/ScriptableObject/SODemo_CharacterData", order = 1)]
public class SODemo_CharacterData : ScriptableObject
{
    [SerializeField]
    public float _health = 100.0f;

    [SerializeField]
    public float _maxhealth = 100.0f;
}
