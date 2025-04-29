using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GameBlackboard
{


    public Dictionary<string, object> entries;

    public GameBlackboard()
    {
        entries = new Dictionary<string, object>();
    }

    public void Debug()
    {
        //for each entry print out its corresponding key and value
        foreach (var entry in entries)
        {
            string key = entry.Key.ToString();
            UnityEngine.Debug.Log($"Key: {entry.Key}, Value: {entry.Value}");
        }
    }

    //get a list type value or initalize it if it does not exist
    public List<T> GetOrInitList<T>(string key)
    {
        List<T> value = new List<T>();
        if (entries.TryGetValue(key, out object v))
        {
            value = (List<T>)v;
        }
        SetValue(key, value);
        return value;
    }

    //try to get the value of the key
    public bool TryGetValue<T>(string key, out T value)
    {
        if (entries.TryGetValue(key, out object v))
        {
            value = (T) v;
            if (value is not null)
            {
                return true;
            }
        }
        value = default (T);
        return false;
    }

    //change the value in the blackboard entry
    public void SetValue<T>(string key, T value)
    {
        UnityEngine.Debug.Log($"Key: {key}, Value: {value}");
        entries[key] = value;
        UnityEngine.Debug.Log($"the value is {entries[key]}");
        Debug();
    }

    //increase value of an entry
    public void IncreaseValue(string key, int valueToIncrease)
    {
        //get the value
        if (TryGetValue(key, out int previousValue))
        {
            SetValue(key, previousValue + valueToIncrease);
        }
        else
        {
            //create a new entry
            SetValue(key, valueToIncrease);
        }
    }

    public bool ContainsKey(string key) => entries.ContainsKey(key);
    public void RemoveKey(string key) => entries.Remove(key);

}
