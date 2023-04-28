using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;
    private Unit unit;
    private Vector3 targetPosition;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 5f;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            unitAnimator.SetBool(IS_WALKING, true);
        }
        else
        {
            unitAnimator.SetBool(IS_WALKING, false);
        }
    }

    public void Move(GridPosition gridPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }


    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return GetValidActiionGridPositionList().Contains(gridPosition);
    }

    public List<GridPosition> GetValidActiionGridPositionList()
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
}
