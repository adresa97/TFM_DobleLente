public class ActivatePreviewCameraUIEvent
{
    public ActivatePreviewCameraUIEvent() { }
}

public class DeactivatePreviewCameraUIEvent
{
    public DeactivatePreviewCameraUIEvent() { }
}

public class UpdateUIInteractionModeEvent
{
    public UIInteractionModes mode
    {
        get;
        private set;
    }

    public UpdateUIInteractionModeEvent(UIInteractionModes mode)
    {
        this.mode = mode;
    }
}