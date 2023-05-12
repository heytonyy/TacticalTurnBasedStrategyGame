using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    // Member Variables
    [SerializeField] private TextMeshPro textMeshPro;
    private object gridObject;

    // Awake - Start - Update Methods
    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }

    // Virtual Methods
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }
}
