using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraMasterManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents eventListener;

    [SerializeField]
    private GameEvents characterPlayerInputEvents;

    [SerializeField]
    private DepthCameraUpdater shaderUpdater;

    [SerializeField]
    private DepthCameraReplayer memoryUpdater;

    [SerializeField]
    private DepthCameraVisionManager inVisionObjectsManager;

    [SerializeField]
    private DepthCameraConstraintsManager constraints;

    [SerializeField]
    private float maxRecordTime;
    private float recordedTime;
    private float recordTime;

    private float recordInterval = 1.0f/120.0f;

    private bool isCameraActive, isCameraPreview, isCameraRecording;

    private void Start()
    {
        isCameraActive = false;
        isCameraPreview = false;
        isCameraRecording = false;

        recordedTime = 0;
        recordTime = 0;
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
        if (data is InitiatePreviewEvent) ActivatePreview();
        else if (data is CancelPreviewEvent) DeactivatePreview();
        else if (data is InitiateRecordingEvent) StartRecording();
        else if (data is StopRecordingEvent) StopRecording();
        else if (data is ForceStopReplayEvent) DeactivateCamera();
    }

    private void ActivatePreview()
    {
        constraints.ActivateConstraint();
        shaderUpdater.ActivatePreviewCamera();
        isCameraPreview = true;
    }

    /*
    private void ActivatePreview()
    {
        shaderUpdater.DeactivateCamera();
        inVisionObjectsManager.NotifyCameraIsDeactivated();
        constraints.ActivateConstraint();
        isCameraActive = false;

        shaderUpdater.ActivatePreviewCamera();
        isCameraPreview = true;
    }
    */

    private void DeactivatePreview()
    {
        shaderUpdater.DeactivatePreviewCamera();
        isCameraPreview = false;
    }

    private void StartRecording()
    {
        if (!isCameraPreview) ActivatePreview();

        inVisionObjectsManager.NotifyCameraIsRecording();
        isCameraRecording = true;
        recordTime = 0;
        recordedTime = 0;
        StartCoroutine(RecordingLoop());
    }

    private void StopRecording()
    {
        isCameraRecording = false;
    }

    private void ActivateCamera()
    {
        if (!isCameraActive)
        {
            StopRecording();
            DeactivatePreview();

            shaderUpdater.ActivateCamera();
            inVisionObjectsManager.NotifyCameraIsReplaying();
            constraints.DeactivateConstraint();
            isCameraActive = true;
        }
    }

    private void DeactivateCamera()
    {
        isCameraActive = false;
    }

    private void StopReplaying()
    {
        shaderUpdater.DeactivateCamera();
        memoryUpdater.ResetMap();
        inVisionObjectsManager.NotifyCameraIsDeactivated();
        constraints.ActivateConstraint();

        if (isCameraActive) DeactivateCamera();

        characterPlayerInputEvents.Emit(new NotifyReplayHasStopEvent());
    }

    private IEnumerator RecordingLoop()
    {
        while (isCameraRecording && recordTime < maxRecordTime)
        {
            inVisionObjectsManager.NotifyObjectsRecord(recordTime, transform.position);
            memoryUpdater.RecordState(recordTime);

            recordTime += recordInterval;
            yield return new WaitForSecondsRealtime(recordInterval);
        }

        characterPlayerInputEvents.Emit(new NotifyRecordHasStopEvent());

        recordedTime = recordTime;
        recordTime = 0;
        ActivateCamera();
        StartCoroutine(ReplayLoop());
    }

    private IEnumerator ReplayLoop()
    {
        while (isCameraActive && recordTime < recordedTime)
        {
            inVisionObjectsManager.NotifyObjectsReplay(recordTime);
            memoryUpdater.ReplayState(recordTime);

            recordTime += recordInterval;
            yield return new WaitForSecondsRealtime(recordInterval);
        }

        recordedTime = 0;
        recordTime = 0;
        StopReplaying();
    }

    public bool IsCameraActive()
    {
        return isCameraActive;
    }

    public bool IsCameraPreview()
    {
        return isCameraPreview;
    }

    public bool IsCameraRecording()
    {
        return isCameraRecording;
    }
}
