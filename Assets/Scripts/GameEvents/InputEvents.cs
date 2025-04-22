using UnityEngine;
public class InputMoveEvent
{
    public Vector3 moveDirection
    {
        get;
        private set;
    }

    public InputMoveEvent(Vector2 moveDirection)
    {
        this.moveDirection = moveDirection;
    }
}

public class InputRotationEvent
{
    public Vector3 rotationDirection
    {
        get;
        private set;
    }

    public InputRotationEvent(Vector2 rotationDirection)
    {
        this.rotationDirection = rotationDirection;
    }
}

public class InputRunEvent
{
    public bool isRunning
    {
        get;
        private set;
    }

    public InputRunEvent(bool isRunning)
    {
        this.isRunning = isRunning;
    }
}

public class InputJumpEvent
{
    public bool isJumping
    {
        get;
        private set;
    }

    public InputJumpEvent(bool isJumping) 
    {
        this.isJumping = isJumping;
    }
}

public class InputGrabEvent
{
    public InputGrabEvent() { }
}

public class InputPreviewEvent
{
    public InputPreviewEvent() { }
}

public class InputPauseEvent
{
    public InputPauseEvent() { }
}

public class InputResumeEvent
{
    public InputResumeEvent() { }
}

public class InputSchemeChanged
{
    public ControlScheme scheme
    {
        get;
        private set;
    }

    public InputSchemeChanged(ControlScheme scheme)
    {
        this.scheme = scheme;
    }
}

public class InputStartRecordingEvent
{
    public InputStartRecordingEvent() { }
}

public class InputStopRecordingEvent
{
    public InputStopRecordingEvent() { }
}