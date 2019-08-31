using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeManager))]
public class NodeManager_Editor : Editor 
{	
	public override void OnInspectorGUI()
	{

		DrawDefaultInspector();

		NodeManager script = (NodeManager) target;
		if(GUILayout.Button("Create Node Grid"))
		{
			script.CreateNodeGrid();
		}

		if(GUILayout.Button("Clear Node Grid"))
		{
			script.ClearNodeList();
		}
	}
}
