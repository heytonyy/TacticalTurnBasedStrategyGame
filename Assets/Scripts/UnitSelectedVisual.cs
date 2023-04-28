using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private ParticleSystem selectedVisual;

    private void Awake()
    {
        selectedVisual = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            selectedVisual.Play();
        }
        else
        {
            selectedVisual.Stop();
            selectedVisual.Clear(); // Clear particles that are already playing
        }
    }
    
}
