using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CharacterActionsManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents cameraToPlayerEvents;

    [SerializeField]
    private GameEvents playerToCameraEvents;

    [SerializeField]
    private GameEvents UIEvents;

    [SerializeField]
    private CharacterCamerasManager playerCamera;

    [SerializeField]
    private CharacterVisionManager playerVision;

    private bool isGrabbing;
    private bool isInCamera;
    private bool isRecording;
    private bool isReplaying;

    private void Awake()
    {
        isGrabbing = false;
        isInCamera = false;
        isRecording = false;
        isReplaying = false;
    }

    private void OnEnable()
    {
        cameraToPlayerEvents.AddListener(CameraToPlayerEventsCallback);
    }

    private void OnDisable()
    {
        cameraToPlayerEvents.RemoveListener(CameraToPlayerEventsCallback);
    }

    private void CameraToPlayerEventsCallback(object data)
    {
        if (data is NotifyRecordHasStopEvent) WhenStopRecordReceived();
        else if (data is NotifyReplayHasStopEvent) WhenStopReplayReceived();
    }

    private void WhenStopRecordReceived()
    {
        CloseCameraMode();
        isRecording = false;
        isReplaying = true;
    }

    private void WhenStopReplayReceived()
    {
        isReplaying = false;
    }

    private void OpenCameraMode()
    {
        playerCamera.SetPreviewCamera();
        playerVision.DeactivateVision(true);
        UIEvents.Emit(new ActivatePreviewCameraUIEvent());
        isInCamera = true;
    }

    private void CloseCameraMode()
    {
        playerCamera.SetRealCamera();
        playerVision.ActivateVision(false);
        UIEvents.Emit(new DeactivatePreviewCameraUIEvent());
        isInCamera = false;
    }

    public void ToggleCameraMode()
    {
        if (isGrabbing) return;

        if (!isInCamera)
        {
            if (isReplaying)
            {
                playerToCameraEvents.Emit(new ForceStopReplayEvent());
            }
            else
            {
                OpenCameraMode();
                playerToCameraEvents.Emit(new InitiatePreviewEvent());
            }
        }
        else
        {
            CloseCameraMode();
            playerToCameraEvents.Emit(new CancelPreviewEvent());
        }
    }

    public void StartRecording()
    {
        if (isInCamera && !isRecording)
        {
            playerToCameraEvents.Emit(new InitiateRecordingEvent());
            isRecording = true;
        }
    }

    public void StopRecording()
    {
        if (isRecording)
        {
            playerToCameraEvents.Emit(new StopRecordingEvent());
        }
    }

    public void InteractObject()
    {
        if (isInCamera) return;

        if (!isGrabbing)
        {
            GrabOrInteractObject();
        }
        else
        {
            DropObject();
        }
    }

    private void GrabOrInteractObject()
    {
        if (playerVision.InteractWithObject())
        {
            isGrabbing = true;
        }
    }

    private void DropObject()
    {
        playerVision.InteractWithObject();
        isGrabbing = false;
    }
}
