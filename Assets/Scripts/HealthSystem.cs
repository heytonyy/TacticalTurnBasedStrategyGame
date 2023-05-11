using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    // Event Handlers
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    // Member Variables
    [SerializeField] private int health = 100;
    private int healthMax;

    // Awake - Start - Update Methods
    private void Awake()
    {
        healthMax = health;
    }

    // Getter Methods
    public float GetHealthNormalized() => (float)health / healthMax;

    // Class Methods
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0; // Prevents health from going negative
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

}
