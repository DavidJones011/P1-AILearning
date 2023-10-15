using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace AILearning
{
    public enum EntryType
    {
        INT,
        FLOAT,
        BOOL,
        VECTOR,
        COLOR,
    }

    // Class used to hold static function for creating blackboard entries
    public class BlackboardEntryFactory
    {
        public static BlackboardEntry Create(EntryType entryType)
        {
            BlackboardEntry entry = new BlackboardEntry();
            entry._entryType = entryType;

            switch(entryType)
            {
                case EntryType.INT:
                    entry._value._integer = 0;
                    break;
                case EntryType.FLOAT:
                    entry._value._float = 0.0f;
                    break;
                case EntryType.VECTOR:
                    entry._value._vector3 = Vector3.zero;
                    break;
                case EntryType.BOOL:
                    entry._value._bool = false;
                    break;
                case EntryType.COLOR:
                    entry._value._color = Color.white;
                    break;
                default:
                    throw new NotImplementedException("Invalid entry type. Wasn't implemented");
            }

            return entry;
        }
    }

    // Structure that holds data for a blackboard entry
    // -- Blackboard entries can vary
    public struct BlackboardEntry
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct BBEntryUnion
        {
            [FieldOffset(0)] public int _integer;
            [FieldOffset(0)] public float _float;
            [FieldOffset(0)] public Vector3 _vector3;
            [FieldOffset(0)] public Color _color;
            [FieldOffset(0)] public bool _bool;
        };

        internal EntryType _entryType;
        internal BBEntryUnion _value;
    }

    // Blackboard is in charge of holding data to be used for decision making.
    // -- 
    public class Blackboard : MonoBehaviour
    {
        Dictionary<string, BlackboardEntry> entries = new Dictionary<string, BlackboardEntry>();

        // creates and registers a blackboard entry with associated name
        public void RegisterBlackboardEntry(string name, EntryType type)
        {
            BlackboardEntry entry = BlackboardEntryFactory.Create(type);
            entries.Add(name, entry);
        }

        // creates and unregisters a blackboard entry with associated name
        public void UnregisterBlackboardEntry(string name)
        {
            entries.Remove(name);
        }

        public void SetBoolValue(string key, bool value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key]; // how do i get a reference here?

            if (entry._entryType != EntryType.BOOL)
                return;

            entry._value._bool = value;
            entries[key] = entry;
        }

        public bool GetBoolValue(string key, ref bool value)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return false;

            if (entry._entryType != EntryType.BOOL)
                return false;

            value = entry._value._bool;
            return true;
        }

        public void SetColorValue(string key, Color value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key];

            if (entry._entryType != EntryType.COLOR)
                return;

            entry._value._color = value;
            entries[key] = entry;
        }

        public bool GetColorValue(string key, ref Color value)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return false;

            if (entry._entryType != EntryType.COLOR)
                return false;

            value = entry._value._color;
            return true;
        }

        public void SetVectorValue(string key, Vector3 value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key]; // how do i get a reference here?

            if (entry._entryType != EntryType.VECTOR)
                return;

            entry._value._vector3 = value;
            entries[key] = entry;
        }

        public bool GetVectorValue(string key, ref Vector3 value)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return false;

            if (entry._entryType != EntryType.VECTOR)
                return false;

            value = entry._value._vector3;
            return true;
        }

        public void SetIntValue(string key, int value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key]; // how do i get a reference here?

            if (entry._entryType != EntryType.INT)
                return;

            entry._value._integer = value;
            entries[key] = entry;
        }

        public bool GetIntValue(string key, ref int value)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return false;

            if (entry._entryType != EntryType.INT)
                return false;

            value = entry._value._integer;
            return true;
        }

        public void SetFloatValue(string key, float value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key]; // how do i get a reference here?

            if (entry._entryType != EntryType.FLOAT)
                return;

            entry._value._float = value;
            entries[key] = entry;
        }

        public bool GetFloatValue(string key, ref float value)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return false;

            if (entry._entryType != EntryType.FLOAT)
                return false;

            value = entry._value._float;
            return true;
        }
    }
}
