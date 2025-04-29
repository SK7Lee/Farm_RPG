using UnityEngine;

public struct BlackboardEntryData 
{
    //name of the key on the blackboard
    public string keyName;
    //there are the values that are to be supported
    public enum ValueType { Int, Float, Bool, String, Vector3 }

    public ValueType valueType;

    //since we can't serialise object directly, we will need to store the different values separately
    public int intValue;
    public float floatValue;
    public bool boolValue;
    public string stringValue;
    public Vector3 vector3Value;

    //function to get value
    public object GetValue()
    {
        switch (valueType)
        {
            case ValueType.Int:
                return intValue;
            case ValueType.Float: 
                return floatValue;
            case ValueType.Bool: 
                return boolValue;
            case ValueType.String:  
                return stringValue;
            case ValueType.Vector3:
                return vector3Value;
            default:
                return null;
        }
    }
}
