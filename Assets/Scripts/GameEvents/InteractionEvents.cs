public class NoObjectOnVisionEvent
{
    public NoObjectOnVisionEvent() { }
}

public class InteractableObjectOnVisionEvent
{
    public InteractableObjectOnVisionEvent() { }
}

public class GrabableObjectOnVisionEvent
{
    public GrabableObjectOnVisionEvent() { }
}

public class ObjectGrabbedEvent
{
    public ObjectGrabbedEvent() { }
}

public class ActivateSignalEvent
{
    public int signal
    {
        get;
        private set;
    }

    public ActivateSignalEvent(int signal)
    {
        this.signal = signal;
    }
}

public class DeactivateSignalEvent
{
    public int signal
    {
        get;
        private set;
    }

    public DeactivateSignalEvent(int signal)
    {
        this.signal = signal;
    }
}

public class TakeCameraEvent
{
    public TakeCameraEvent() { }
}