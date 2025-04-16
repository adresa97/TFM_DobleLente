using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public enum UIInteractionModes { EMPTY = 0, INTERACT = 1, GRAB = 2, DROP = 3 };

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents UIEvents;

    [SerializeField]
    private GameObject regularPanel;

    [SerializeField]
    private GameObject previewPanel;

    [SerializeField]
    private Image interactionIcon;

    [SerializeField]
    private Sprite[] interactionModesActions;

    private void OnEnable()
    {
        UIEvents.AddListener(UIEventsCallback);
    }

    private void OnDisable()
    {
        UIEvents.RemoveListener(UIEventsCallback);
    }

    private void UIEventsCallback(object data)
    {
        if (data is ActivatePreviewCameraUIEvent) ActivatePreviewPanel();
        else if (data is DeactivatePreviewCameraUIEvent) DeactivatePreviewPanel();
        else if (data is UpdateUIInteractionModeEvent) ChangeInteractionActionIcon((data as UpdateUIInteractionModeEvent).mode);
    }

    private void ActivatePreviewPanel()
    {
        regularPanel.SetActive(false);
        previewPanel.SetActive(true);
    }

    private void DeactivatePreviewPanel()
    {
        regularPanel.SetActive(true);
        previewPanel.SetActive(false);
    }

    private void ChangeInteractionActionIcon(UIInteractionModes modeChange)
    {
        switch(modeChange)
        {
            case UIInteractionModes.EMPTY:
                interactionIcon.enabled = false;
                break;

            case UIInteractionModes.INTERACT:
                interactionIcon.sprite = interactionModesActions[1];
                interactionIcon.enabled = true;
                break;

            case UIInteractionModes.GRAB:
                interactionIcon.sprite = interactionModesActions[2];
                interactionIcon.enabled = true;
                break;

            case UIInteractionModes.DROP:
                interactionIcon.sprite = interactionModesActions[3];
                interactionIcon.enabled = true;
                break;

            default:
                break;
        }
    }
}
