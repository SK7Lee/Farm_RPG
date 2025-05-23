using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



[CustomPropertyDrawer(typeof(BlackboardCondition))]
public class BlackboardConditionEditor : PropertyDrawer
{
    SerializedProperty valueType;

    TextField keyNameField;
    DropdownField valueTypeField;
    Foldout blackboardEntryFoldout;
    DropdownField comparisonField;

    IntegerField intValueField;
    FloatField floatValueField;
    TextField stringValueField;
    Toggle boolValueField;
    Vector3Field vector3ValueField;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        HandleUtility.Repaint();
    }

    //value fields
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        //create property container element
        var container = new VisualElement();

        //get the visual tree by loading in the uxml file forr this view
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Elements/Editor/BlackboardCondition.uxml");
        VisualElement tree = visualTree.Instantiate();
        container.Add(tree);

        //blackboardentrydata property
        SerializedProperty blackboardEntry = property.FindPropertyRelative("blackboardEntryData");
        SerializedProperty keyName = blackboardEntry.FindPropertyRelative("keyName");
        valueType = blackboardEntry.FindPropertyRelative("valueType");

        //value properties
        SerializedProperty intValue = blackboardEntry.FindPropertyRelative("intValue");
        SerializedProperty floatValue = blackboardEntry.FindPropertyRelative("floatValue");
        SerializedProperty stringValue = blackboardEntry.FindPropertyRelative("stringValue");
        SerializedProperty boolValue = blackboardEntry.FindPropertyRelative("boolValue");
        SerializedProperty vector3Value = blackboardEntry.FindPropertyRelative("vector3Value");

        //comparison property
        SerializedProperty comparison = property.FindPropertyRelative("comparison");

        //bind keyname and valuetype to their fields
        keyNameField = container.Q<TextField>("key-name");
        valueTypeField = container.Q<DropdownField>("value-type");
        blackboardEntryFoldout = container.Q<Foldout>("blackboard-entry-data");

        //bind the values fields
        intValueField = container.Q<IntegerField>("int-value");
        intValueField.BindProperty(intValue);
        
        floatValueField = container.Q<FloatField>("float-value");
        floatValueField.BindProperty(floatValue);

        stringValueField = container.Q<TextField>("string-value");
        stringValueField.BindProperty(stringValue);

        boolValueField = container.Q<Toggle>("bool-value");
        boolValueField.BindProperty(boolValue);

        vector3ValueField = container.Q<Vector3Field>("vector3-value");
        vector3ValueField.BindProperty(vector3Value);

        //bind the comparison dropdown
        comparisonField = container.Q<DropdownField>("comparison");

        comparisonField.BindProperty(comparison);
        keyNameField.BindProperty(keyName);
        valueTypeField.BindProperty(valueType);

        keyNameField.RegisterValueChangedCallback(OnKeyNameChange);
        valueTypeField.RegisterValueChangedCallback(OnValueTypeChange);

        return container;

    }

    private void OnValueTypeChange(ChangeEvent<string> evt)
    {
        //remove class from all
        intValueField.RemoveFromClassList("selected");
        floatValueField.RemoveFromClassList("selected");
        stringValueField.RemoveFromClassList("selected");
        boolValueField.RemoveFromClassList("selected");
        vector3ValueField.RemoveFromClassList("selected");

        switch ((BlackboardEntryData.ValueType)valueType.enumValueIndex)
        {
            case BlackboardEntryData.ValueType.Int:
                intValueField.AddToClassList("selected");
                break;
            case BlackboardEntryData.ValueType.Float:
                floatValueField.AddToClassList("selected");
                break;
            case BlackboardEntryData.ValueType.Bool:
                boolValueField.AddToClassList("selected");
                break;
            case BlackboardEntryData.ValueType.String:
                stringValueField.AddToClassList("selected");
                break;
            case BlackboardEntryData.ValueType.Vector3:
                vector3ValueField.AddToClassList("selected");
                break;

        }

    }

    private void OnKeyNameChange(ChangeEvent<string> evt)
    {
        blackboardEntryFoldout.text = evt.newValue;
    }
}
