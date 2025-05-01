using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(CutsceneProcessData))]
public class CutsceneProcessDataEditor : PropertyDrawer
{
    ListView actionListView;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        HandleUtility.Repaint();
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        //create property container element
        var container = new VisualElement();

        //get the visual tree by loading in the uxml file for this view
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Elements/Editor/BlackboardCondition.uxml");
        VisualElement tree = visualTree.Instantiate();
        container.Add(tree);

        SerializedProperty actionList = property.FindPropertyRelative("actions");
        //display cutscene
        actionListView = container.Q<ListView>("action-list-view");
        actionListView.reorderable = true;
        actionListView.reorderMode = ListViewReorderMode.Animated;
        actionListView.showAddRemoveFooter = true;

        return base.CreatePropertyGUI(property);
    }

}
