using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructableCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}
