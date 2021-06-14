using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloorManager))]
public class FloorManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        GUIStyle Style = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.white
            },
            fontSize = 18
        };
        FloorManager floorManager = (FloorManager)target;

        if (floorManager == null)
        {
            return;
        }

        Handles.Label(floorManager.transform.position + Vector3.up * 5, $"Acive Section: {floorManager.ActiveSection}", Style);

        foreach (FloorSection section in floorManager.ActiveFloorSections.Values)
        {
            Handles.Label(section.transform.position, $"World Index: {section.Index}\r\nPosition: {section.transform.position}", Style);
            Handles.DrawWireCube(section.transform.position - new Vector3(4, -1.5f, -4), new Vector3(10, 2, 10));
        }
    }
}
