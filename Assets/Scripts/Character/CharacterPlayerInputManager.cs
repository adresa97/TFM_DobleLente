using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class CharacterPlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents inputEvents;

    [SerializeField]
    private CharacterActionsManager actionManager;

    private bool isInPauseMode;

    Vector2 moveAxis;
    Vector2 lookAtAxis;
    private bool isRunningButtonPressed;
    private bool isJumpingButtonPressed;

    private void Awake()
    {
        moveAxis = Vector3.zero;

        isInPauseMode = false;

        isRunningButtonPressed = false;
        isJumpingButtonPressed = false;
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
        if (isInPauseMode) OnPauseModeCallback(data);
        else OnRegularModeCallback(data);
    }

    private void OnPauseModeCallback(object data)
    {
        if (data is InputPauseEvent) isInPauseMode = true;
    }

    private void OnRegularModeCallback(object data)
    {
        if (data is InputPauseEvent) isInPauseMode = false;
        else if (data is InputMoveEvent) moveAxis = (data as InputMoveEvent).moveDirection;
        else if (data is InputRotationEvent) lookAtAxis = (data as InputRotationEvent).rotationDirection;
        else if (data is InputRunEvent) isRunningButtonPressed = (data as InputRunEvent).isRunning;
        else if (data is InputJumpEvent) isJumpingButtonPressed = (data as InputJumpEvent).isJumping;
        else if (data is InputGrabEvent) actionManager.InteractObject();
        else if (data is InputPreviewEvent) actionManager.ToggleCameraMode();
        else if (data is InputStartRecordingEvent) actionManager.StartRecording();
        else if (data is InputStopRecordingEvent) actionManager.StopRecording();
    }

    public Vector2 GetMovementAxis() { return moveAxis; }
    public Vector2 GetLookAtAxis() { return lookAtAxis; }

    public bool IsRunningButtonPressed() { return isRunningButtonPressed; }
    public bool IsJumpButtonPressed() { return isJumpingButtonPressed; }

    public bool IsGamePaused() { return false; }
}
