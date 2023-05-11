using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Member Variables
    [SerializeField] private bool invert;
    private Transform cameraTransform;

    // Awake - Start - Update Methods
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate() // LateUpdate is called after Update
    {
        if (invert)
        {
            Vector3 directionToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + directionToCamera * -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}
