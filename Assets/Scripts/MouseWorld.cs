using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    // Singleton?
    private static MouseWorld instance;

    // Member Variables
    [SerializeField] private LayerMask mousePlaneLayerMask;

    // Awake - Start - Update Methods
    private void Awake()
    {
        instance = this;
    }

    // Class Methods
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return raycastHit.point;
    }

}
