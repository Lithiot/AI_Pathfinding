using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public bool drawGizmos = false;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        CreateGrid();
    }

    public Vector2 size;
    public LayerMask unwakableMask;
    public float nodeRadius;
    public Node[,] grid;
    public GameObject nodePrefab;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    public void CreateGrid()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(size.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(size.y / nodeDiameter);

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * size.x / 2 - Vector3.forward * size.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(pos, nodeRadius, unwakableMask));
                /*
                GameObject obj = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
                grid[x, y] = obj.GetComponent<Node>();
                grid[x, y].LoadData(walkable, pos, new Vector2Int(x, y));
                */
                grid[x, y] = new Node(walkable, pos, new Vector2Int(x, y));
             }
        }

        LoadNeighbours();
    }

    private void LoadNeighbours()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y].Walkable)
                {
                    // Agrego la tile de arriba
                    if(nodeIsValid(x, y + 1))
                        grid[x, y].Neighbours.Add(grid[x, y + 1]);
                    // Agrego la tile de abajo
                    if(nodeIsValid(x, y - 1))
                        grid[x, y].Neighbours.Add(grid[x, y - 1]);
                    // Agrego la tile de la izquierda
                    if (nodeIsValid(x - 1, y))
                        grid[x, y].Neighbours.Add(grid[x - 1, y]);
                    // Agrego la tile de la derecha
                    if (nodeIsValid(x + 1, y))
                        grid[x, y].Neighbours.Add(grid[x + 1, y]);
                }
            }
        }
    }

    private bool nodeIsValid(int x, int y)
    {
        if (x >= 0 && x < gridSizeX)
            if (y >= 0 && y < gridSizeY)
                return grid[x, y].Walkable;

        return false;
    }

    public void ClearGrid()
    {
        Array.Clear(grid, 0, grid.Length);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, 1.0f, size.y));

            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.Walkable) ? Color.green : Color.red;
                    switch (n.Walkable)
                    {
                        case true:
                            Gizmos.DrawWireCube(n.PosInWorld, Vector3.one * (nodeDiameter - 0.2f));
                            break;
                        case false:
                            Gizmos.DrawCube(n.PosInWorld, Vector3.one * (nodeDiameter - 0.2f));
                            break;
                    }
                }
            }
        }
    }
}
