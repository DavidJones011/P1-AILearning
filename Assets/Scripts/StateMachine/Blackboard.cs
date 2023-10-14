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
        VECTOR,
    }

    public class BlackboardEntryFactory
    {
        public static BlackboardEntry Create(EntryType entryType)
        {
            BlackboardEntry entry = new BlackboardEntry();
            entry._entryType = entryType;

            switch(entryType)
            {
                case EntryType.INT:
                    entry._value._i_value = 0;
                    break;
                case EntryType.FLOAT:
                    entry._value._f_value = 0.0f;
                    break;
                case EntryType.VECTOR:
                    entry._value._v_value = Vector3.zero;
                    break;
                default:
                    throw new NotImplementedException("Invalid entry type. Wasn't implemented");
            }

            return entry;
        }
    }

    public struct BlackboardEntry
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct BBEntryUnion
        {
            [FieldOffset(0)] public int _i_value;
            [FieldOffset(0)] public float _f_value;
            [FieldOffset(0)] public Vector3 _v_value;
        };

        internal EntryType _entryType;
        internal BBEntryUnion _value;
    }

    // Blackboard is in charge of holding data to be used for decision making.
    // -- 
    public class Blackboard : MonoBehaviour
    {
        Dictionary<string, BlackboardEntry> entries = new Dictionary<string, BlackboardEntry>();

        public void RegisterBlackboardEntry(string name, EntryType type)
        {
            BlackboardEntry entry = BlackboardEntryFactory.Create(type);
            entries.Add(name, entry);
        }

        public void UnregisterBlackboardEntry(string name)
        {
            entries.Remove(name);
        }

        public void SetVectorValue(string key, Vector3 value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key]; // how do i get a reference here?

            if (entry._entryType != EntryType.VECTOR)
                return;

            entry._value._v_value = value;
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

            value = entry._value._v_value;
            return true;
        }

        public void SetIntValue(string key, int value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key]; // how do i get a reference here?

            if (entry._entryType != EntryType.INT)
                return;

            entry._value._i_value = value;
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

            value = entry._value._i_value;
            return true;
        }

        public void SetFloatValue(string key, float value)
        {
            if (!entries.ContainsKey(key))
                return;

            BlackboardEntry entry = entries[key]; // how do i get a reference here?

            if (entry._entryType != EntryType.FLOAT)
                return;

            entry._value._f_value = value;
            entries[key] = entry;
        }

        public bool GetFloatValue(string key, float value)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return false;

            if (entry._entryType != EntryType.FLOAT)
                return false;

            value = entry._value._f_value;
            return true;
        }

        /*private T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data.Length);
            ms.Read(data, 0, data.Length);
            return (T)ms.ReadByte();
        }*/

        /*public byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }*/

        /*private bool GetEntry(string key, EntryType type, ref byte[] data)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return false;

            if (entry._entryType != type)
                return false;

            data = entry.data;
            return true;
        }*/

        /*private void SetEntry<T>(string key, EntryType type, T value)
        {
            BlackboardEntry entry;
            bool found = entries.TryGetValue(key, out entry);

            if (!found)
                return;

            if (entry._entryType != type)
                return;

            switch(type)
            {
                case EntryType.FLOAT:
                    entry._value._f_value = value;
                    break;
                case EntryType.INT:
                    break;
                case EntryType.VECTOR:
                    break;
                default:
                    return;
            }

            entry.data = data;
        }*/
    }
}
