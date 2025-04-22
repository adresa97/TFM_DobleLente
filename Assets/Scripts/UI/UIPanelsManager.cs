using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelsManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents inputEvents;

    [SerializeField]
    private GameEvents playerEvents;

    [SerializeField]
    private GameEvents cameraEvents;

    [SerializeField]
    private GameObject regularPanel;

    [SerializeField]
    private UIRegularPanelManager regularPanelManager;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private GameObject previewPanel;


    private void OnEnable()
    {
        inputEvents.AddListener(InputEventsCallback);
        playerEvents.AddListener(PlayerEventsCallback);
        cameraEvents.AddListener(CameraEventsCallback);
    }

    private void OnDisable()
    {
        inputEvents.RemoveListener(InputEventsCallback);
        playerEvents.RemoveListener(PlayerEventsCallback);
        cameraEvents.RemoveListener(CameraEventsCallback);
    }

    private void InputEventsCallback(object data)
    {
        if (data is InputPauseEvent) ActivatePausePanel();
        else if (data is InputResumeEvent) DeactivatePausePanel();
    }

    private void PlayerEventsCallback(object data)
    {
        if (data is InitiatePreviewEvent) ActivatePreviewPanel();
        else if (data is CancelPreviewEvent) DeactivatePreviewPanel(false);
    }

    private void CameraEventsCallback(object data)
    {
        if (data is NotifyRecordHasStopEvent) DeactivatePreviewPanel(true);
    }

    private void ActivatePreviewPanel()
    {
        regularPanel.SetActive(false);
        previewPanel.SetActive(true);
    }

    private void DeactivatePreviewPanel(bool isRecording)
    {
        regularPanel.SetActive(true);
        regularPanelManager.InitUI(isRecording);
        previewPanel.SetActive(false);
    }

    private void ActivatePausePanel()
    {
        pausePanel.SetActive(true);
    }

    private void DeactivatePausePanel()
    {
        pausePanel.SetActive(false);
    }
}
