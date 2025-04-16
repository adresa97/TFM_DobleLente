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