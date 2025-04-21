using Codice.CM.WorkspaceServer.Tree.GameUI.Checkin.Updater;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class UIRegularPanelManager : MonoBehaviour
{
    private enum UIInteractionIcon { NONE = 0, INTERACT = 1, GRAB = 2, DROP = 3 };
    private enum UICameraIcon { STOP = 0, CAMERA = 1 };

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

    private void OnEnable()
    {
        playerEvents.AddListener(PlayerEventsCallback);
        cameraEvents.AddListener(CameraEventsCallback);
    }

    private void OnDisable()
    {
        playerEvents.RemoveListener(PlayerEventsCallback);
        cameraEvents.RemoveListener(CameraEventsCallback);
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

        if (isRecording) cameraModeIcon.sprite = cameraModesActions[1];
        else cameraModeIcon.sprite = cameraModesActions[0];
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
            case UICameraIcon.STOP:
                cameraModeIcon.sprite = cameraModesActions[1];
                break;

            case UICameraIcon.CAMERA:
                cameraModeIcon.sprite = cameraModesActions[0];
                break;

            default:
                break;
        }
    }
}
