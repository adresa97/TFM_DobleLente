using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents inputEvents;

    [SerializeField]
    private GameEvents characterPlayerInputEvents;

    [SerializeField]
    private GameEvents depthCameraEvents;

    [SerializeField]
    private GameEvents UIEvents;

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
        characterPlayerInputEvents.AddListener(CharacterPlayerInputEventsCallback);
    }

    private void OnDisable()
    {
        inputEvents.RemoveListener(InputEventsCallback);
        characterPlayerInputEvents.AddListener(CharacterPlayerInputEventsCallback);
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

    private void CharacterPlayerInputEventsCallback(object data)
    {
        if (data is NotifyReplayHasStopEvent) isPlaying = false;
        else if (data is NotifyRecordHasStopEvent) StopRecordingCameraMode();
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
                depthCameraEvents.Emit(new ForceStopReplayEvent());
            }
            else
            {
                playerCamera.SetPreviewCamera();
                depthCameraEvents.Emit(new InitiatePreviewEvent());
                UIEvents.Emit(new ActivatePreviewCameraUIEvent());
                isPreviewMode = true;
            }
        }
        else if (!isRecording)
        {
            playerCamera.SetRealCamera();
            depthCameraEvents.Emit(new CancelPreviewEvent());
            UIEvents.Emit(new DeactivatePreviewCameraUIEvent());
            isPreviewMode = false;
        }
    }

    private void StartRecording()
    {
        if (isPreviewMode)
        {
            depthCameraEvents.Emit(new InitiateRecordingEvent());
            isPreviewMode = false;
            isRecording = true;
        }
    }

    private void StopRecording()
    {
        if (isRecording)
        {
            //StopRecordingCameraMode();
            depthCameraEvents.Emit(new StopRecordingEvent());
        }
    }

    private void StopRecordingCameraMode()
    {
        playerCamera.SetRealCamera();
        UIEvents.Emit(new DeactivatePreviewCameraUIEvent());
        isRecording = false;
        isPlaying = true;
    }
}
