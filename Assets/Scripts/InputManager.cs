using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents inputEvents;

    private Vector2 moveDirection;
    private Vector2 rotateDirection;
    private bool isRunning;
    private bool isJumping;
    private bool isGrabbing;
    private bool isPreviewing;
    private bool isRecording;

    private void Awake()
    {
        moveDirection = Vector2.zero;
        rotateDirection = Vector2.zero;

        isRunning = false;
        isJumping = false;
        isGrabbing = false;
        isPreviewing = false;
        isRecording = false;
    }

    // When moving action is executed
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>().normalized;

        if (context.started && inputDirection != moveDirection)
        {
            moveDirection = inputDirection;
        }
        else if (context.canceled)
        {
            moveDirection = Vector2.zero;
        }
        else if (inputDirection != moveDirection)
        {
            moveDirection = inputDirection;
        }

        inputEvents.Emit(new InputMoveEvent(moveDirection));
    }

    // When rotate action is executed
    public void OnRotate(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();

        if (context.started && inputDirection != rotateDirection)
        {
            rotateDirection = inputDirection;
        }
        else if (context.canceled)
        {
            rotateDirection = Vector2.zero;
        }

        inputEvents.Emit(new InputRotationEvent(rotateDirection));
    }

    // When run action is executed
    public void OnRun(InputAction.CallbackContext context)
    {
        bool mustRun = context.ReadValue<float>() != 0.0f ? true : false;

        if (context.started && mustRun != isRunning)
        {
            isRunning = mustRun;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
        else if (mustRun != isRunning)
        {
            isRunning = mustRun;
        }

        inputEvents.Emit(new InputRunEvent(isRunning));
    }

    // When jump action is executed
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !isJumping)
        {
            isJumping = true;
            inputEvents.Emit(new InputJumpEvent(isJumping));
        }
        else if (context.canceled)
        {
            isJumping = false;
            inputEvents.Emit(new InputJumpEvent(isJumping));
        }
    }

    // When grab action is executed
    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started && !isGrabbing)
        {
            isGrabbing = true;
            inputEvents.Emit(new InputGrabEvent());
        }
        else if (context.canceled)
        {
            isGrabbing = false;
        }
    }

    // When preview action is executed
    public void OnPreviewing(InputAction.CallbackContext context)
    {
        if (context.started && !isPreviewing)
        {
            isPreviewing = true;
            inputEvents.Emit(new InputPreviewEvent());
        }
        else if (context.canceled)
        {
            isPreviewing = false;
        }
    }

    // When record action is executed
    public void OnRecording(InputAction.CallbackContext context)
    {
        if (context.started && !isRecording)
        {
            isRecording = true;
            inputEvents.Emit(new InputStartRecordingEvent());
        }
        else if (context.canceled)
        {
            isRecording = false;
            inputEvents.Emit(new InputStopRecordingEvent());
        }
    }
}
