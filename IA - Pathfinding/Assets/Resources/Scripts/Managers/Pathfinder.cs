using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathfindingMethod
{
    BreathFirst, DepthFirst
}

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder instance;

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

    public bool drawGizmos = false;
    public PathfindingMethod method = PathfindingMethod.BreathFirst;
    public Vector2Int origin;
    public Vector2Int target;

    private List<Node> openedList = new List<Node>();
    private List<Node> visitedList = new List<Node>();
    private Stack<Node> path = new Stack<Node>();
    public Node[,] grid;

    private Node targetNode;
    private Node originNode;

    private void Start()
    {
        grid = GridManager.instance.grid;
    }

    private void Update()
    {
        targetNode = grid[target.x, target.y];
        originNode = grid[origin.x, origin.y];
    }

    public void Pathfind(Node origin, Node target)
    {
        ClearForNewPathfinding();

        switch (method)
        {
            case PathfindingMethod.BreathFirst:
                StartCoroutine(BreathFirst(origin, target));
                break;
            case PathfindingMethod.DepthFirst:
                StartCoroutine(DepthFirst(origin, target));
                break;
        }
    }

    private void ClearForNewPathfinding()
    {
        openedList.Clear();
        visitedList.Clear();
        path.Clear();

        foreach (Node n in grid)
        {
            n.Parent = null;
        }
    }

    private IEnumerator BreathFirst(Node origin, Node target)
    {
        if (origin == target)
        {
            Debug.Log("There's no path to be found!");
            yield break;
        }

        OpenNeighbours(origin);
        while (openedList.Count > 0)
        {
            Node currentNode = openedList[0];

            if (currentNode == target)
            {
                Debug.Log("I found the path!");
                CreatePath(currentNode, origin);
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.04f);

            OpenNeighbours(currentNode);
        }

        Debug.Log("The path could not be found, maybe the position is unavaliable?");
        yield break;
    }

    private IEnumerator DepthFirst(Node origin, Node target)
    {
        if (origin == target)
        {
            Debug.Log("There's no path to be found!");
            yield break;
        }

        OpenNeighbours(origin);
        while (openedList.Count > 0)
        {
            Node currentNode = openedList[openedList.Count - 1];

            if (currentNode == target)
            {
                Debug.Log("I found the path!");
                CreatePath(currentNode, origin);
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.04f);

            OpenNeighbours(currentNode);
        }

        Debug.Log("The path could not be found, maybe the position is unavaliable?");
        yield break;
    }

    private void OpenNeighbours(Node node)
    {
        foreach (Node n in node.Neighbours)
        {
            if (!openedList.Contains(n) && !visitedList.Contains(n))
            {
                openedList.Add(n);
                n.Parent = node;
            }
        }

        if (openedList.Contains(node))
        {
            openedList.Remove(node);
            visitedList.Add(node);
        }
    }

    private void CreatePath(Node node, Node origin)
    {
        path.Push(node);

        while (node.Parent != origin)
        {
            path.Push(node.Parent);
            node = node.Parent;
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.cyan;
            foreach (Node n in openedList)
            {
                Gizmos.DrawCube(n.PosInWorld, Vector3.one * ((GridManager.instance.nodeRadius * 2) - 0.2f));
            }

            Gizmos.color = Color.gray;
            foreach (Node n in visitedList)
            {
                Gizmos.DrawCube(n.PosInWorld, Vector3.one * ((GridManager.instance.nodeRadius * 2) - 0.2f));
            }

            Gizmos.color = Color.blue;
            foreach (Node n in path)
            {
                Gizmos.DrawCube(n.PosInWorld, Vector3.one * ((GridManager.instance.nodeRadius * 2) - 0.2f));
            }

            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(targetNode.PosInWorld, Vector3.one * ((GridManager.instance.nodeRadius * 2) - 0.2f));
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(originNode.PosInWorld, Vector3.one * ((GridManager.instance.nodeRadius * 2) - 0.2f));
        }
    }
}
