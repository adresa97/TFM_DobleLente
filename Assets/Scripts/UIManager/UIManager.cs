using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents UIEvents;

    [SerializeField]
    private GameObject previewPanel;

    private void OnEnable()
    {
        UIEvents.AddListener(UIEventsCallback);
    }

    private void OnDisable()
    {
        UIEvents.RemoveListener(UIEventsCallback);
    }

    private void UIEventsCallback(object data)
    {
        if (data is ActivatePreviewCameraUIEvent) ActivatePreviewPanel();
        else if (data is DeactivatePreviewCameraUIEvent) DeactivatePreviewPanel();
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
