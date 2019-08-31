using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
	public enum State
	{
		Idle, Opened, Closed
	}

	public Node parent;
	public State state = State.Idle;
	private Color nodeGizmosColor = Color.black;
	[SerializeField] private List<GameObject> adjacentNodes;
	[SerializeField] private LayerMask nodeMask;

	private void Start()
	{
		CheckForAdjacents();
	}

	public void CheckForAdjacents()
	{	
		float myDistance = NodeManager.instance.distance;

		if(!NodeManager.instance.canUseDiagonals)
			myDistance -= 0.2f;

		if(!Physics.Raycast(transform.position, Vector3.up, NodeManager.instance.distance))
		{
			Collider[] posibleAdjacents = Physics.OverlapSphere(transform.position, myDistance, nodeMask);
			
			RaycastHit hitInfo;
			for (int i = 0; i < posibleAdjacents.Length; i++)
			{
				Vector3 dir = posibleAdjacents[i].transform.position - transform.position;

				if(Physics.Raycast(transform.position, dir, out hitInfo, NodeManager.instance.distance))
					if(hitInfo.collider.CompareTag("Node") && hitInfo.collider.gameObject != this.gameObject)
						adjacentNodes.Add(posibleAdjacents[i].gameObject);
			}	
		}

	}

	public void ClearAdjacents()
	{
		for (int i = 0; i < adjacentNodes.Count; i++)
		{
			Destroy(adjacentNodes[i]);
		}

		adjacentNodes.Clear();
	}
	
	private void OnDrawGizmos()
	{
		switch(adjacentNodes.Count)
		{
			case 0:
				Gizmos.color = Color.red;
				break;
			case 1:
				Gizmos.color = Color.magenta;
				break;
			case 2:
				Gizmos.color = Color.cyan;
				break;
			case 3:
				Gizmos.color = Color.green;
				break;
			case 4:
				Gizmos.color = Color.black;
				break;
		}


		Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

		foreach (GameObject n in adjacentNodes)
		{
			Gizmos.DrawLine(transform.position, n.transform.position);
		}
	}
}
