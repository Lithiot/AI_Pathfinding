using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour 
{
	public static NodeManager instance;
	
	[SerializeField] private Vector2 size;
	[Range(1, 4)] public int distance = 1;
	[SerializeField] private GameObject nodePrefab;
	[SerializeField] public bool canUseDiagonals = false;
	[SerializeField] private List<GameObject> nodeList;

	private void Awake()
	{
		if (instance && instance != this)
			Destroy(this);
		else
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
	}

	public void CreateNodeGrid()
	{
		Vector3 firstNodePos = new Vector3(transform.position.x - (size.x / 2), 0.0f, transform.position.z - (size.y / 2));

		for (int z = (int)firstNodePos.z; z <= size.y / 2; z += distance)
		{
			for (int x = (int)firstNodePos.x; x <= size.x / 2; x += distance)
			{
				Vector3 pos = new Vector3(x, 0.0f, z);
				Vector3 dir = (pos + Vector3.up) - pos;

				RaycastHit hitInfo;
				if(Physics.Raycast(pos - Vector3.up, dir, out hitInfo))
				{
					Debug.Log("I hitted: " + hitInfo.collider.name);
					Debug.DrawRay(pos - Vector3.up, dir * hitInfo.distance, Color.green, 10.0f, true);
				}
				else
				{
					GameObject obj = Instantiate(nodePrefab, pos, Quaternion.identity, this.transform);
					obj.name = "Node[" + nodeList.Count + "]";
					nodeList.Add(obj);
					Debug.DrawRay(pos - Vector3.up, dir * distance, Color.cyan, 10.0f, true);
				}
			}
		}
	}

	public void ClearNodeList()
	{
		for (int i = 0; i < nodeList.Count; i++)
		{
			DestroyImmediate(nodeList[i]);
		}

		nodeList.Clear();
	}

	public void CheckConnections()
	{
		foreach(GameObject n in nodeList)
		{
			n.GetComponent<Node>().CheckForAdjacents();
		}
	}

	public void ResetConnections()
	{
		foreach(GameObject n in nodeList)
		{
			n.GetComponent<Node>().ClearAdjacents();
			n.GetComponent<Node>().CheckForAdjacents();
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position, new Vector3(size.x, 0.0f, size.y));
	}
	
}
