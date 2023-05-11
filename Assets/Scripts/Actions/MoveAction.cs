using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    // Event Handlers
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    // Member Variables
    [SerializeField] private int maxMoveDistance = 4;
    private Vector3 targetPosition;

    // Awake - Start - Update Methods
    protected override void Awake()
    {
        base.Awake(); // Calls the Awake() method in the BaseAction class
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float stoppingDistance = 0.1f;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 5f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        else
        {
            OnStopMoving?.Invoke(this, EventArgs.Empty);

            ActionComplete();
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(
            transform.forward,
            moveDirection,
            Time.deltaTime * rotateSpeed
        );
    }

    // Abstract Method Implementation
    public override string GetActionName() => "Move";

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue; // validation: cant move to grid position out of bounds
                }

                if (unitGridPosition == testGridPosition)
                {
                    continue; // validation: cant move to unit's current position
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue; // validation: cant move to grid position occupied by another unit
                }

                validActionGridPositionList.Add(testGridPosition);
            }
        }

        return validActionGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

}
