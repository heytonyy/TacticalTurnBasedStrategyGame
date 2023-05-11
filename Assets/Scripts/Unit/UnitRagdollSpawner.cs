using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform ragdollPrefab;

    [SerializeField]
    private Transform originalRootBone;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    // if issues, subscribe to OnDead in Awake() instead of Start()
    private void Start()
    {
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(
            ragdollPrefab,
            transform.position,
            transform.rotation
        );
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
