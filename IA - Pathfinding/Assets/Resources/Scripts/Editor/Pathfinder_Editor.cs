using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pathfinder))]
public class Pathfinder_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Pathfinder script = (Pathfinder)target;
        if (GUILayout.Button("find the path!"))
        {
            script.Pathfind(script.grid[script.origin.x, script.origin.y], script.grid[script.target.x, script.target.y]);
        }
    }
}
