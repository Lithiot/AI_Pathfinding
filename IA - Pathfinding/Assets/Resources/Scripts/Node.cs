using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    [SerializeField] bool walkable;
    [SerializeField] Vector3 posInWorld;
    [SerializeField] Vector2Int posInArray;
    [SerializeField] List<Node> neighbours;
    [SerializeField] Node parent;

    public Node(bool walkable, Vector3 positionInWorld, Vector2Int positionInArray)
    {
        this.walkable = walkable;
        this.posInWorld = positionInWorld;
        this.posInArray = positionInArray;
        neighbours = new List<Node>();
        parent = null;
    }

    public void LoadData(bool walkable, Vector3 positionInWorld, Vector2Int positionInArray)
    {
        this.walkable = walkable;
        this.posInWorld = positionInWorld;
        this.posInArray = positionInArray;
        neighbours = new List<Node>();
        parent = null;
    }

    public bool Walkable { get => walkable; }
    public Vector3 PosInWorld { get => posInWorld; }
    public List<Node> Neighbours { get => neighbours; }
    public Vector2Int PosInArray { get => posInArray; }
    public Node Parent { get => parent; set => parent = value; }
}
