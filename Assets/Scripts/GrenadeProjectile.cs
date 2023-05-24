using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{
    // Event Handlers
    public static event EventHandler OnAnyGrenadeExploded;

    // Member Variables
    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;

    // Awake - Start - Update Methods
    private void Update()
    {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moveDirection * Time.deltaTime * moveSpeed;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1f - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = 0.2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.TakeDamage(30);
                }
                if (collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
                {
                    destructableCrate.Damage();
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.SetParent(null);

            Vector3 explosionOffset = Vector3.up * 1f;
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + explosionOffset, Quaternion.identity);

            Destroy(gameObject);

            onGrenadeBehaviourComplete?.Invoke();
        }
    }

    // Class Methods
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;

        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0f;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
