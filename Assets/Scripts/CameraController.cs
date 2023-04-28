using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_OFFSET_Y = 2f;
    private const float MAX_FOLLOW_OFFSET_Y = 12f;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x += 1f;
        }
        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;

        float moveSpeed = 10f;

        transform.position += moveVector * Time.deltaTime * moveSpeed;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y -= 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y += 1f;
        }

        float rotationSpeed = 100f;

        transform.eulerAngles += rotationVector * Time.deltaTime * rotationSpeed;
    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_OFFSET_Y, MAX_FOLLOW_OFFSET_Y);

        float zoomSpeed = 5f;

        cinemachineTransposer.m_FollowOffset =
            Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

}
