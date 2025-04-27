using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocationManager))]
public class LocationManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Output Coords"))
        {
            GameObject markerParent = GameObject.Find("NPC Makers");
            foreach (Transform point in markerParent.transform)
            {
                //output information of each marker
                Debug.Log(point.gameObject.name + " " + point.position + " " + point.rotation.eulerAngles);
            }
        }
    }
}
