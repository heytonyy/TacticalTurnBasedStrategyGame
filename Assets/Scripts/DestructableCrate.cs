using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;
    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() => gridPosition;

    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, Quaternion.identity);
        ApplyExplosionToChidren(crateDestroyedTransform, 150f, transform.position, 10f);
        Destroy(gameObject);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChidren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            ApplyExplosionToChidren(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
