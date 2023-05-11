using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionComplete;

        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                // get unit references
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 shootDirection = (
                    targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()
                ).normalized;

                // above head camera height offset
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                // shoulder camera offset
                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset =
                    Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

                // complete camera position
                Vector3 actionCameraPosition =
                    shooterUnit.GetWorldPosition()
                    + cameraCharacterHeight
                    + shoulderOffset
                    + (shootDirection * -1);

                // set camera position and rotation
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(
                    shooterUnit.GetWorldPosition() + cameraCharacterHeight
                );

                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionComplete(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }
}
