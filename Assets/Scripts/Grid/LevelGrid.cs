using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition;

    private GridSystem gridSystem;

    [SerializeField]
    private Transform gridDebugObjectPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public void UnitMovedGridPosition(
        Unit unit,
        GridPosition fromGridPosition,
        GridPosition toGridPosition
    )
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit) =>
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit) =>
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition) =>
        gridSystem.GetGridObject(gridPosition).GetUnitList();

    public GridPosition GetGridPosition(Vector3 worldPosition) =>
        gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) =>
        gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool IsValidGridPosition(GridPosition gridPosition) =>
        gridSystem.IsValidGridPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HadAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
}
