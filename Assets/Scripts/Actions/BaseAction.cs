using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    // Event Handlers
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    // Member Variables
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    // Awake - Start - Update Methods
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    // Abstract Methods (implemented by subclasses)
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public abstract List<GridPosition> GetValidActionGridPositionList();

    // Virtual Methods (overridden by subclasses)
    public virtual int GetActionPointsCost() => 1;
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList().Contains(gridPosition);
    }


    // Action Delegate Methods
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete?.Invoke();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    // Getter Methods
    public Unit GetUnit() => unit;

}
