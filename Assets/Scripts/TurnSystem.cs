using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
    // Singleton
    public static TurnSystem Instance { get; private set; }

    // Event Handlers
    public event EventHandler OnTurnChanged;

    // Member Variables
    private int turnNumber = 1;
    private bool isPlayer = true;

    // Awake - Start - Update Methods
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Getter Methods
    public int GetTurnNumber() => turnNumber;
    public bool IsPlayerTurn() => isPlayer;

    // Class Methods
    public void NextTurn()
    {
        turnNumber++;
        isPlayer = !isPlayer;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

}
