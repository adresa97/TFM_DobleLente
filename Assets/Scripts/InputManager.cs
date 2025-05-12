using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private GameEvents inputEvents;

    [SerializeField]
    private GameEvents buttonEvents;

    [SerializeField]
    private PlayerInput input;

    [SerializeField]
    private string menuMap;

    [SerializeField]
    private string gameMap;

    [SerializeField]
    private string keyboardScheme;

    [SerializeField]
    private string gamepadScheme;

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

        Time.timeScale = 1;
    }

    private void Start()
    {
        ChangeControls(input);
    }

    private void ButtonEventsCallback(object data)
    {
        if (data is ResumeButtonEvent)
        {
            Resume();
        }
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
        else if (inputDirection != rotateDirection)
        {
            rotateDirection = inputDirection;
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

    // When pause action is executed
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && input.currentActionMap.name != menuMap)
        {
            inputEvents.Emit(new InputPauseEvent());
            input.SwitchCurrentActionMap(menuMap);
            Time.timeScale = 0;

            buttonEvents.AddListener(ButtonEventsCallback);
        }
    }

    // Back to play
    public void Resume()
    {
        inputEvents.Emit(new InputResumeEvent());
        input.SwitchCurrentActionMap(gameMap);
        Time.timeScale = 1;

        buttonEvents.RemoveListener(ButtonEventsCallback);
    }

    public void ChangeControls(PlayerInput playerInput)
    {
        if (playerInput.currentControlScheme.Equals(keyboardScheme))
        {
            inputEvents.Emit(new InputSchemeChanged(Utils.ControlScheme.KEYBOARD));
            playerData.SaveActiveScheme(Utils.ControlScheme.KEYBOARD);
        }
        else if (playerInput.currentControlScheme.Equals(gamepadScheme))
        {
            inputEvents.Emit(new InputSchemeChanged(Utils.ControlScheme.GAMEPAD));
            playerData.SaveActiveScheme(Utils.ControlScheme.GAMEPAD);
        }
    }
}