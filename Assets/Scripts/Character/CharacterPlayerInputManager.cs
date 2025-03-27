using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents inputEvents;

    [SerializeField]
    private GameEvents depthCameraEvents;

    [SerializeField]
    private CharacterCamerasManager playerCamera;

    Vector2 moveAxis;
    Vector2 lookAtAxis;
    private bool isRunningButtonPressed;
    private bool isJumpingButtonPressed;
    private bool isPreviewMode;
    private bool isRecording;
    private bool isPlaying;

    private void Awake()
    {
        moveAxis = Vector3.zero;

        isRunningButtonPressed = false;
        isJumpingButtonPressed = false;
        isPreviewMode = false;
        isRecording = false;
        isPlaying = false;
    }

    private void OnEnable()
    {
        inputEvents.AddListener(InputEventsCallback);
    }

    private void OnDisable()
    {
        inputEvents.RemoveListener(InputEventsCallback);
    }

    private void InputEventsCallback(object data)
    {
        if (data is InputMoveEvent) moveAxis = (data as InputMoveEvent).moveDirection;
        else if (data is InputRotationEvent) lookAtAxis = (data as InputRotationEvent).rotationDirection;
        else if (data is InputRunEvent) isRunningButtonPressed = (data as InputRunEvent).isRunning;
        else if (data is InputJumpEvent) isJumpingButtonPressed = (data as InputJumpEvent).isJumping;
        else if (data is InputGrabEvent) GrabDrop();
        else if (data is InputPreviewEvent) TogglePreviewMode();
        else if (data is InputStartRecordingEvent) StartRecording();
        else if (data is InputStopRecordingEvent) StopRecording();
    }

    public Vector2 GetMovementAxis() { return moveAxis; }
    public Vector2 GetLookAtAxis() { return lookAtAxis; }

    public bool IsRunningButtonPressed() { return isRunningButtonPressed; }
    public bool IsJumpButtonPressed() { return isJumpingButtonPressed; }

    public bool IsGamePaused() { return false; }

    private void GrabDrop()
    {

    }

    private void TogglePreviewMode()
    {
        if (!isPreviewMode)
        {
            if (isPlaying)
            {
                depthCameraEvents.Emit(new DeactivateCameraEvent());
                isPlaying = false;
            }
            else
            {
                playerCamera.SetPreviewCamera();
                depthCameraEvents.Emit(new ActivatePreviewCameraEvent());
                isPreviewMode = true;
            }
        }
        else if (!isRecording)
        {
            playerCamera.SetRealCamera();
            depthCameraEvents.Emit(new DeactivatePreviewCameraEvent());
            isPreviewMode = false;
        }
    }

    private void StartRecording()
    {
        if (isPreviewMode)
        {
            isRecording = true;
        }
    }

    private void StopRecording()
    {
        if (isRecording && isPreviewMode)
        {
            playerCamera.SetRealCamera();
            depthCameraEvents.Emit(new ActivateCameraEvent());
            isRecording = false;
            isPreviewMode = false;
            isPlaying = true;
        }
    }
}
