using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraMasterManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents gameplayEvents;

    [SerializeField]
    private GameEvents playerToCameraEvents;

    [SerializeField]
    private GameEvents cameraToPlayerEvents;

    [SerializeField]
    private DepthCameraUpdater shaderUpdater;

    [SerializeField]
    private DepthCameraReplayer memoryUpdater;

    [SerializeField]
    private DepthCameraVisionManager inVisionObjectsManager;

    [SerializeField]
    private DepthCameraConstraintsManager constraints;

    [SerializeField]
    private AudioSource soundPlayer;

    [SerializeField]
    private float maxRecordTime;
    private float recordedTime;
    private float recordTime;

    private int lastSecond;
    private float recordInterval = 1.0f/60.0f;

    private bool isCameraActive, isCameraPreview, isCameraRecording;

    private void Start()
    {
        isCameraActive = false;
        isCameraPreview = false;
        isCameraRecording = false;

        recordedTime = 0;
        recordTime = 0;

        lastSecond = 0;
    }

    private void OnEnable()
    {
        gameplayEvents.AddListener(GameplayEventsCallback);
        playerToCameraEvents.AddListener(PlayerToCameraEventsCallback);
    }

    private void OnDisable()
    {
        gameplayEvents.RemoveListener(GameplayEventsCallback);
        playerToCameraEvents.RemoveListener(PlayerToCameraEventsCallback);
    }

    private void GameplayEventsCallback(object data)
    {
        if (data is TakeCameraEvent)
        {
            constraints.SetPlayerConstraint();
            StartCoroutine(WaitToStop());
        }
        else if (data is InitiateRecordingEvent)
        {
            RemoteRecording(2);
        }
    }

    private void PlayerToCameraEventsCallback(object data)
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

    private void RemoteRecording(int seconds)
    {
        inVisionObjectsManager.NotifyCameraIsRecording();
        isCameraRecording = true;
        recordTime = 0;
        recordedTime = 0;
        StartCoroutine(RemoteRecordingLoop(seconds));
    }

    private IEnumerator WaitToStop()
    {
        yield return new WaitForSecondsRealtime(0.05f);

        StopAllActions();
    }

    private void StopAllActions()
    {
        StopAllCoroutines();
        recordedTime = 0;
        recordTime = 0;
        lastSecond = 0;

        isCameraActive = false;
        isCameraPreview = false;
        isCameraRecording = false;

        StopReplaying();
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

        cameraToPlayerEvents.Emit(new NotifyReplayHasStopEvent());
    }

    private IEnumerator RecordingLoop()
    {
        while (isCameraRecording && recordTime < maxRecordTime)
        {
            inVisionObjectsManager.NotifyObjectsRecord(recordTime, transform.position);
            memoryUpdater.RecordState(recordTime);

            recordTime += recordInterval;

            if (recordTime > lastSecond + 1)
            {
                cameraToPlayerEvents.Emit(new NotifySecondHasBeenRecordedEvent());
                soundPlayer.Play();
                lastSecond++;
            }

            yield return new WaitForSecondsRealtime(recordInterval);
        }

        cameraToPlayerEvents.Emit(new NotifyRecordHasStopEvent());

        recordedTime = recordTime;
        recordTime = 0;
        lastSecond = 0;
        ActivateCamera();
        StartCoroutine(ReplayLoop());
    }

    private IEnumerator RemoteRecordingLoop(int seconds)
    {
        while (isCameraRecording && recordTime < seconds)
        {
            inVisionObjectsManager.NotifyObjectsRecord(recordTime, transform.position);

            recordTime += recordInterval;

            if (recordTime > lastSecond + 1)
            {
                soundPlayer.Play();
                lastSecond++;
            }

            yield return new WaitForSecondsRealtime(recordInterval);
        }

        recordedTime = recordTime;
        recordTime = 0;
        lastSecond = 0;

        shaderUpdater.ActivateCamera();
        inVisionObjectsManager.NotifyCameraIsReplaying();
        isCameraActive = true;

        StartCoroutine(RemoteReplayLoop());
    }

    private IEnumerator ReplayLoop()
    {
        while (isCameraActive && recordTime < recordedTime)
        {
            inVisionObjectsManager.NotifyObjectsReplay(recordTime);
            memoryUpdater.ReplayState(recordTime);

            recordTime += recordInterval;

            if (recordTime > lastSecond + 1)
            {
                soundPlayer.Play();
                lastSecond++;
            }

            yield return new WaitForSecondsRealtime(recordInterval);
        }

        recordedTime = 0;
        recordTime = 0;
        lastSecond = 0;
        StopReplaying();
    }

    private IEnumerator RemoteReplayLoop()
    {
        while (isCameraActive && recordTime < recordedTime)
        {
            inVisionObjectsManager.NotifyObjectsReplay(recordTime);

            recordTime += recordInterval;

            if (recordTime > lastSecond + 1)
            {
                soundPlayer.Play();
                lastSecond++;
            }

            yield return new WaitForSecondsRealtime(recordInterval);
        }

        recordedTime = 0;
        recordTime = 0;
        lastSecond = 0;

        shaderUpdater.DeactivateCamera();
        inVisionObjectsManager.NotifyCameraIsDeactivated();
        isCameraActive = false;

        cameraToPlayerEvents.Emit(new NotifyRemoteReplayHasStopEvent());

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
