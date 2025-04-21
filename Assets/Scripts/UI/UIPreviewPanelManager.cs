using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class UIPreviewPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents playerEvents;

    [SerializeField]
    private GameEvents cameraEvents;

    [SerializeField]
    private UITimer timer;

    [SerializeField]
    private UIRecordSignal recordingUI;

    [SerializeField]
    private Image cameraRecordPlayIcon;

    [SerializeField]
    private Sprite[] cameraRecordPlayActions;

    [SerializeField]
    private Image cameraBackIcon;

    private bool isRecording;

    private void OnEnable()
    {
        playerEvents.AddListener(PlayerEventsCallback);
        cameraEvents.AddListener(CameraEventsCallback);

        InitPreviewUI();
    }

    private void OnDisable()
    {
        playerEvents.RemoveListener(PlayerEventsCallback);
        cameraEvents.RemoveListener(CameraEventsCallback);

        ResetPreviewUI();
    }

    private void PlayerEventsCallback(object data)
    {
        if (data is InitiateRecordingEvent) ChangeToRecordingUI();
    }

    private void CameraEventsCallback(object data)
    {
        if (data is NotifySecondHasBeenRecordedEvent) IncrementTimer();
    }

    private void InitPreviewUI()
    {
        cameraBackIcon.enabled = true;
        cameraRecordPlayIcon.sprite = cameraRecordPlayActions[0];
        isRecording = false;
    }

    private void ChangeToRecordingUI()
    {
        if (!isRecording)
        {
            recordingUI.StartRecordingSymbols();
            timer.InitTimer();
            cameraBackIcon.enabled = false;
            cameraRecordPlayIcon.sprite = cameraRecordPlayActions[1];
            isRecording = true;
        }
    }

    private void IncrementTimer()
    {
        if (isRecording) timer.IncrementTimer();
    }

    private void ResetPreviewUI()
    {
        recordingUI.StopRecordingSymbols();
        timer.ResetTimer();
        cameraBackIcon.enabled = false;
        isRecording = false;
    }
}
