using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] private TextMeshPro textMeshPro;

    private void Update()
    {
        // textMeshPro.text = gridObject.ToString();
        textMeshPro.text = "";

    }
    
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }
}
