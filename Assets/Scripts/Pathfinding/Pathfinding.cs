using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // Constants
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    // Singleton
    public static Pathfinding Instance { get; private set; }

    // Member Variables
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obsticlesLayerMask;
    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

    // Awake - Start - Update Methods
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one Pathfinding! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = gridSystem.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(
                  worldPosition + Vector3.down * raycastOffsetDistance,
                  Vector3.up,
                  raycastOffsetDistance * 2,
                  obsticlesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        // initialize the pathNode values
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        // set the startNode values
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        // find the path
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList); // optimized by choosing the lowest fCost node

            if (currentNode == endNode)
            {
                return CalculatePath(endNode); // reached the end
            }

            // move the current node from the open list to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // check the neighbours of the current node
            foreach (PathNode neighbourNode in GetNeighborList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue; // already searched this node
                }

                if (!neighbourNode.IsWalkable())
                {
                    // not walkable, so add to the closed list and skip
                    closedList.Add(neighbourNode);
                    continue;
                }

                // heuristic cost to move from the current node to the neighbour node
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                // replace if tenativeGCost is better
                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // no path found
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        int moveDiagonalCost = MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance);
        int moveStraightCost = MOVE_STRAIGHT_COST * remaining;

        return moveDiagonalCost + moveStraightCost;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];

        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostNode.GetFCost())
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0) // validate that the node is within the grid
        {
            // Left Node
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // Left Down Node
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Left Up Node
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth()) // validate that the node is within the grid
        {
            // Right Node
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // Right Down Node
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Right Up Node
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.z - 1 >= 0) // validate that the node is within the grid
        {
            // Down Node
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        if (gridPosition.z + 1 < gridSystem.GetHeight()) // validate that the node is within the grid
        {
            // Up Node
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);

        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }
}
