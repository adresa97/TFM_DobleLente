using UnityEngine;
using UnityEngine.UI;

public class UIRegularPanelManager : MonoBehaviour
{
    private enum UIInteractionIcon { NONE = 0, INTERACT = 1, GRAB = 2, DROP = 3 };
    private enum UICameraIcon { NONE = 0, STOP = 1, CAMERA = 2 };

    [SerializeField]
    private GameEvents gameplayEvents;

    [SerializeField]
    private GameEvents playerEvents;

    [SerializeField]
    private GameEvents cameraEvents;

    [SerializeField]
    private Image interactionIcon;

    [SerializeField]
    private Sprite[] interactionModesActions;

    [SerializeField]
    private Image cameraModeIcon;

    [SerializeField]
    private Sprite[] cameraModesActions;

    private void Start()
    {
        cameraModeIcon.enabled = false;
    }

    private void OnEnable()
    {
        gameplayEvents.AddListener(GameplayEventsCallback);
        playerEvents.AddListener(PlayerEventsCallback);
        cameraEvents.AddListener(CameraEventsCallback);
    }

    private void OnDisable()
    {
        gameplayEvents.RemoveListener(GameplayEventsCallback);
        playerEvents.RemoveListener(PlayerEventsCallback);
        cameraEvents.RemoveListener(CameraEventsCallback);
    }

    private void GameplayEventsCallback(object data)
    {
        if (data is TakeCameraEvent)
        {
            SetCameraIcon(UICameraIcon.CAMERA);
        }
    }

    private void PlayerEventsCallback(object data)
    {
        if (data is NoObjectOnVisionEvent) SetInteractIcon(UIInteractionIcon.NONE);
        else if (data is InteractableObjectOnVisionEvent) SetInteractIcon(UIInteractionIcon.INTERACT);
        else if (data is GrabableObjectOnVisionEvent) SetInteractIcon(UIInteractionIcon.GRAB);
        else if (data is ObjectGrabbedEvent) SetInteractIcon(UIInteractionIcon.DROP);
    }

    private void CameraEventsCallback(object data)
    {
        if (data is NotifyRecordHasStopEvent) SetCameraIcon(UICameraIcon.STOP);
        else if (data is NotifyReplayHasStopEvent) SetCameraIcon(UICameraIcon.CAMERA);
    }

    public void InitUI(bool isRecording)
    {
        interactionIcon.enabled = false;

        if (isRecording) SetCameraIcon(UICameraIcon.STOP);
        else SetCameraIcon(UICameraIcon.CAMERA);
    }

    private void SetInteractIcon(UIInteractionIcon icon)
    {
        switch(icon)
        {
            case UIInteractionIcon.NONE:
                interactionIcon.enabled = false;
                break;

            case UIInteractionIcon.INTERACT:
                interactionIcon.sprite = interactionModesActions[1];
                interactionIcon.enabled = true;
                break;

            case UIInteractionIcon.GRAB:
                interactionIcon.sprite = interactionModesActions[2];
                interactionIcon.enabled = true;
                break;

            case UIInteractionIcon.DROP:
                interactionIcon.sprite = interactionModesActions[3];
                interactionIcon.enabled = true;
                break;

            default:
                break;
        }
    }

    private void SetCameraIcon(UICameraIcon icon)
    {
        switch(icon)
        {
            case UICameraIcon.NONE:
                cameraModeIcon.enabled = false;
                break;

            case UICameraIcon.STOP:
                cameraModeIcon.sprite = cameraModesActions[2];
                cameraModeIcon.enabled = true;
                break;

            case UICameraIcon.CAMERA:
                cameraModeIcon.sprite = cameraModesActions[1];
                cameraModeIcon.enabled = true;
                break;

            default:
                break;
        }
    }
}
