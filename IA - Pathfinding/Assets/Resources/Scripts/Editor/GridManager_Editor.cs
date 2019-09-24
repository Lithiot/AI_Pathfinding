using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManager script = (GridManager)target;
        if (GUILayout.Button("Create Grid"))
        {
            script.CreateGrid();
        }
        if (GUILayout.Button("Clear Grid"))
        {
            script.ClearGrid();
        }
    }
}
