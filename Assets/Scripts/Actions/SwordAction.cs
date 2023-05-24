using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    // Member Variables
    private int maxSwordDistance = 1;

    // Awake - Start - Update Methods
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        ActionComplete();
    }

    // Abstract Method Implementation
    public override string GetActionName() => "Sword";

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
            // actionValue = 200, // if enemy is in range, prioritize this action
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue; // cant sword to grid position out of bounds
                }

                validActionGridPositionList.Add(testGridPosition);
            }
        }

        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log("SwordAction.TakeAction");
        ActionStart(onActionComplete);
    }
}
