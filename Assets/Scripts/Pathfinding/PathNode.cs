using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    // Member Variables
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool isWalkable = true;

    // Constructor
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    // Getter Methods
    public GridPosition GetGridPosition() => gridPosition;
    public int GetGCost() => gCost;
    public int GetHCost() => hCost;
    public int GetFCost() => fCost;
    public PathNode GetCameFromPathNode() => cameFromPathNode;
    public bool IsWalkable() => isWalkable;

    // Setter Methods
    public void SetGCost(int gCost) => this.gCost = gCost;
    public void SetHCost(int hCost) => this.hCost = hCost;
    public void CalculateFCost() => fCost = gCost + hCost;
    public void SetCameFromPathNode(PathNode pathNode) => cameFromPathNode = pathNode;
    public void ResetCameFromPathNode() => cameFromPathNode = null;
    public void SetIsWalkable(bool isWalkable) => this.isWalkable = isWalkable;
}
