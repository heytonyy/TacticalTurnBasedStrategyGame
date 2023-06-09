using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    // Singleton
    public static UnitActionSystem Instance { get; private set; }

    // Event Handlers
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    // Member Variables
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    private BaseAction selectedAction;
    private bool isBusy;

    // Awake - Start - Update Methods
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
        {
            return; // don't do anything if we're doing an action
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return; // don't do anything if it's the enemy's turn
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // don't do anything if the mouse is over a UI element
        }

        if (TryHandleUnitSelection())
        {
            return; // don't do anything if we're selecting a unit
        }

        HandleSeletedAction();
    }

    private void HandleSeletedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(
                MouseWorld.GetPosition()
            );

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return; // don't do anything if we're clicking on an invalid grid position
            }

            if (!selectedUnit.TrySpendActionToTakeAction(selectedAction))
            {
                return; // don't do anything if we can't spend the action points
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        return false; // don't do anything if we're clicking on the same unit
                    }

                    if (unit.IsEnemy())
                    {
                        return false; // don't do anything if we're clicking on an enemy unit
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    // Getter / Setter Methods
    public Unit GetSelectedUnit() => selectedUnit;
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>()); // default action (using generics)

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsBusy() => isBusy;
    private void SetBusy()
    {
        isBusy = true;

        OnBusyChanged?.Invoke(this, isBusy);
    }
    private void ClearBusy()
    {
        isBusy = false;

        OnBusyChanged?.Invoke(this, isBusy);
    }

    public BaseAction GetSelectedAction() => selectedAction;

    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
}
