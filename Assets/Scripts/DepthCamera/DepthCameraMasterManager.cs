using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraMasterManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents eventListener;

    [SerializeField]
    private DepthCameraUpdater shaderUpdater;

    [SerializeField]
    private DepthCameraVisionManager inVisionObjectsManager;

    [SerializeField]
    private DepthCameraConstraintsManager constraints;

    private bool isCameraActive, isCameraPreview;

    private void Start()
    {
        isCameraActive = false;
        isCameraPreview = false;
    }

    private void OnEnable()
    {
        eventListener.AddListener(DepthCameraEventsCallback);
    }

    private void OnDisable()
    {
        eventListener.RemoveListener(DepthCameraEventsCallback);
    }

    private void DepthCameraEventsCallback(object data)
    {
        if (data is ActivateCameraEvent) ActivateCamera();
        else if (data is DeactivateCameraEvent) DeactivateCamera();
        else if (data is ActivatePreviewCameraEvent) ActivatePreview();
        else if (data is DeactivatePreviewCameraEvent) DeactivatePreview();
    }

    private void ActivateCamera()
    {
        shaderUpdater.DeactivatePreviewCamera();
        isCameraPreview = false;

        shaderUpdater.ActivateCamera();
        inVisionObjectsManager.NotifyCameraIsActivated();
        constraints.DeactivateConstraint();
        isCameraActive = true;
    }

    private void DeactivateCamera()
    {
        shaderUpdater.DeactivateCamera();
        inVisionObjectsManager.NotifyCameraIsDeactivated();
        constraints.ActivateConstraint();
        isCameraActive = false;
    }

    private void ActivatePreview()
    {
        shaderUpdater.DeactivateCamera();
        inVisionObjectsManager.NotifyCameraIsDeactivated();
        constraints.ActivateConstraint();
        isCameraActive = false;

        shaderUpdater.ActivatePreviewCamera();
        isCameraPreview = true;
    }

    private void DeactivatePreview()
    {
        shaderUpdater.DeactivatePreviewCamera();
        isCameraPreview = false;
    }

    public bool IsCameraActive()
    {
        return isCameraActive;
    }

    public bool IsCameraPreview()
    {
        return isCameraPreview;
    }
}
