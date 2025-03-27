using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents depthCameraEvents;

    [SerializeField]
    private GameObject previewPanel;

    private void OnEnable()
    {
        depthCameraEvents.AddListener(DepthCameraEventsCallback);
    }

    private void OnDisable()
    {
        depthCameraEvents.RemoveListener(DepthCameraEventsCallback);
    }

    private void DepthCameraEventsCallback(object data)
    {
        if (data is ActivatePreviewCameraEvent) ActivatePreviewPanel();
        else if (data is DeactivatePreviewCameraEvent) DeactivatePreviewPanel();
        else if (data is ActivateCameraEvent) DeactivatePreviewPanel();
    }

    private void ActivatePreviewPanel()
    {
        previewPanel.SetActive(true);
    }

    private void DeactivatePreviewPanel()
    {
        previewPanel.SetActive(false);
    }
}
