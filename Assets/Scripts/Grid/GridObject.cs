using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    // Member Variables
    private GridPosition gridPosition;
    private GridSystem<GridObject> gridSystem;
    private List<Unit> unitList;

    // Constructor
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    // Class Methods
    public List<Unit> GetUnitList() => unitList;
    public void AddUnit(Unit unit) => unitList.Add(unit);
    public void RemoveUnit(Unit unit) => unitList.Remove(unit);
    public Unit GetUnit()
    {
        if (HadAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }
    public bool HadAnyUnit() => unitList.Count > 0;

}
