using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    // Member Variables
    private GridObject gridObject;
    [SerializeField] private TextMeshPro textMeshPro;

    // Awake - Start - Update Methods
    private void Update()
    {
        textMeshPro.text = gridObject.ToString();
        // toggle comment below to see gridObject.ToString() in the game
        // textMeshPro.text = "";
    }

    // Setter Methods
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }
}
