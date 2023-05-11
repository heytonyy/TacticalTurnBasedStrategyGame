using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit shootingUnit;
        public Unit targetUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private State state;

    private int maxShootDistance = 7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShoot;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (
                    targetUnit.GetWorldPosition() - unit.GetWorldPosition()
                ).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(
                    transform.forward,
                    aimDirection,
                    Time.deltaTime * rotateSpeed
                );
                break;
            case State.Shooting:
                if (canShoot)
                {
                    Shoot();
                    canShoot = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float cooloffStateTime = 0.5f;
                stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override string GetActionName() => "Shoot";

    private void Shoot()
    {
        OnShoot?.Invoke(
            this,
            new OnShootEventArgs { shootingUnit = unit, targetUnit = targetUnit }
        );

        int damageAmount = 40;
        targetUnit.TakeDamage(damageAmount);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShoot = true;

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue; // cant move to grid position out of bounds
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue; // cant shoot outside circle of max distance
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue; // cant shoot at empty grid position
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue; // cant shoot at friendly unit, "same team" -> .IsEnemy() will be the same
                }

                validActionGridPositionList.Add(testGridPosition);
            }
        }

        return validActionGridPositionList;
    }

    public Unit GetTargetUnit() => targetUnit;

    public int GetMaxShootDistance() => maxShootDistance;
}
