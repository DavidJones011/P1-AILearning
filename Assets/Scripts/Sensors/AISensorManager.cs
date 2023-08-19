using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AILearning
{
    /// \brief Id that gets generated when registering a sense.
    public class AISenseID
    {
        public static AISenseID InvalidID = new AISenseID(0xffffffff);

        private const int instanceBits = 24;
        private const int generationBits = 8;

        // id that is generated
        private uint _id;

        public AISenseID(int instanceIndex, byte generationIndex)
        {
            //Assert(instanceIndex < (1 << instanceBits), "instance index needs to fit in 24 bits!");
            _id = (uint)((instanceIndex << generationBits) | generationIndex);
        }

        public AISenseID(uint id)
        {
            _id = id;
        }

        public int GetInstanceIndex() { return ((int)_id >> generationBits); }

        public int GetGenerationIndex() { return ((int)_id & ((1 << generationBits) - 1)); }

        public bool IsValid() { return _id != 0xffffffff; }// -1 if it was an integer

        public override bool Equals(object obj) { return Equals(obj as AISenseID); }

        public bool Equals(AISenseID other) { return other._id == _id; }

        public override int GetHashCode() { return _id.GetHashCode(); }

        public static bool operator ==(AISenseID lhs, AISenseID rhs) { return lhs._id == rhs._id; }

        public static bool operator !=(AISenseID lhs, AISenseID rhs) { return lhs._id != rhs._id; }
    }

    /// \brief Main manager that is responsible for ticking each registered sense.
    ///        There should only be one sensor manager per scene!
    public class AISensorManager : MonoBehaviour
    {
        private static AISensorManager _instance;

        public static AISensorManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("AI Sensor Manager wasn't created!");
                }

                return _instance;
            }
        }

        private List<AISense> _senses = new List<AISense>();

        void RegisterSense(AISense sense)
        {
            AISenseID senseID = sense.GetSenseID();
            if (senseID.IsValid())
            {
                int instanceIndex = senseID.GetInstanceIndex();
                int generationIndex = senseID.GetGenerationIndex();

                sense.SetSenseID(new AISenseID(instanceIndex, (byte)generationIndex));
                _senses[instanceIndex] = sense;
            }
            else
            {
                sense.SetSenseID(new AISenseID(_senses.Count, 0));
                _senses.Add(sense);
            }

            Debug.Log("Registered sense! " + sense.GetName());
        }

        void UnregisterSense(AISenseID senseID)
        {
            if (!senseID.IsValid())
                return;

            int instanceIndex = senseID.GetInstanceIndex();
            AISenseID oldID = _senses[instanceIndex].GetSenseID();

            if (oldID == senseID)
            {
                _senses[instanceIndex] = null; //don't shrink array we just want to deref the sense
            }
        }

        public AISense GetSense(AISenseID senseID)
        {
            if (_senses.Count == 0)
                return null;

            if (senseID.IsValid())
            {
                int index = senseID.GetInstanceIndex();
                return _senses[index];
            }
            return null;
        }

        void OnEnable()
        {
            _instance = this;

            // This would be registered elsewhere but for simplicity I am doing it here
            RegisterSense(new SightSense());
            RegisterSense(new HearSense());
        }

        void Update()
        {
            _instance?.Tick(Time.deltaTime);
        }

        void Tick(float deltaTime)
        {
            foreach (var sense in _senses)
            {
                bool wantsToTick = sense != null && sense.WantsToTick();
                if (wantsToTick)
                {
                    sense.Tick(deltaTime);
                }
            }
        }

        void OnDisable()
        {
            _instance = null;
        }
    }
}
