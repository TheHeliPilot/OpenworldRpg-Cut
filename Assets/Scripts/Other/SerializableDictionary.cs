using System;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using UnityEngine;

public class SerializableDictionarySo<TKey, TValue> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValueEntry> entries;
    private List<TKey> _keys = new List<TKey>();

    public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    [Serializable]
    class KeyValueEntry
    {
        public TKey key;
        public TValue value;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        dictionary.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            dictionary.Add(entries[i].key, entries[i].value);
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if (entries == null)
        {
            return;
        }

        _keys.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            _keys.Add(entries[i].key);
        }

        var result = _keys.GroupBy(x => x)
                         .Where(g => g.Count() > 1)
                         .Select(x => new { Element = x.Key, Count = x.Count() })
                         .ToList();

        if (result.Count <= 0) return;
        string duplicates = string.Join(", ", result);
        Debug.LogError($"Warning {GetType().FullName} keys has duplicates {duplicates}");
    }
}

public class SerializableDictionary<TKey, TValue> : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValueEntry> entries;
    private List<TKey> _keys = new List<TKey>();

    public Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    [Serializable]
    class KeyValueEntry
    {
        public TKey key;
        public TValue value;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        dictionary.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            dictionary.Add(entries[i].key, entries[i].value);
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if (entries == null)
        {
            return;
        }

        _keys.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            _keys.Add(entries[i].key);
        }

        var result = _keys.GroupBy(x => x)
                         .Where(g => g.Count() > 1)
                         .Select(x => new { Element = x.Key, Count = x.Count() })
                         .ToList();

        if (result.Count <= 0) return;
        string duplicates = string.Join(", ", result);
        Debug.LogError($"Warning {GetType().FullName} keys has duplicates {duplicates}");
    }
}