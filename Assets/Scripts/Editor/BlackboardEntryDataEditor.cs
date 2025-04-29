using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(BlackboardEntryData))]
public class BlackboardEntryDataEditor : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        //create property container element
        var container = new VisualElement();

        //get the property of the Keyname and ValueType
        SerializedProperty keyNameProp = property.FindPropertyRelative("keyName");
        SerializedProperty valueTypeProp = property.FindPropertyRelative("valueType");

        //add the keyname and valuetype field
        container.Add(new PropertyField(keyNameProp));
        var valueTypeField = new PropertyField(property.FindPropertyRelative("valueType"));
        container.Add(valueTypeField);

        //the value field (empty for now)
        var valueField = new VisualElement();
        container.Add(valueField);

        //the value of the valuetypr field determines the field to render
        valueTypeField.RegisterValueChangeCallback(evt =>
        {
            valueField.Clear();
            //by default an int field
            string fieldName = "intValue";
            SerializedProperty valueTypeProp = property.FindPropertyRelative("valueType");
            
            //get the correct property name accordingly
            switch ((BlackboardEntryData.ValueType)valueTypeProp.enumValueIndex)
            {
                case BlackboardEntryData.ValueType.Int:
                    fieldName = "intValue";
                    break;
                case BlackboardEntryData.ValueType.Float:
                    fieldName = "floatValue";
                    break;
                case BlackboardEntryData.ValueType.String:
                    fieldName = "stringValue";
                    break;
                case BlackboardEntryData.ValueType.Bool:
                    fieldName = "boolValue";
                    break;
                case BlackboardEntryData.ValueType.Vector3:
                    fieldName = "vector3Value";
                    break;
            }

            //get the correct field
            var value = property.FindPropertyRelative(fieldName);
            var field = new PropertyField(value);
            //have it displayed in the valueField
            valueField.Add(field);

            //force the inspector to update the field
            field.BindProperty(value);

        });

        return container;
    }
}
