using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleTrouble.Utilities
{
    [Serializable]
    public class SerializableTuple<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] private TKey _First;

        [SerializeField] private TValue _Second;

        public TKey first => _First;

        public TValue second => _Second;

        private Tuple<TKey, TValue> _Tuple;

        public SerializableTuple(TKey key, TValue value)
        {
            _Tuple = new Tuple<TKey, TValue>(key, value);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}