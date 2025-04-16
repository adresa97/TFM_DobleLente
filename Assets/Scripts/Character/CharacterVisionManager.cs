using Codice.CM.WorkspaceServer.Tree.GameUI.Checkin.Updater;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisionManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents UIEvents;

    [SerializeField]
    private Transform eye;

    [SerializeField]
    private float viewRange;

    private GameObject onVisionObject;
    private LayerMask interactableObjects;

    private bool isVisionActive;
    private bool isObjectGrabbed;

    private void Start()
    {
        interactableObjects = LayerMask.GetMask("BothWorlds", "RealWorld", "FromOtherWorld");

        onVisionObject = null;

        isVisionActive = true;
        isObjectGrabbed = false;
    }

    private void FixedUpdate()
    {
        if (isVisionActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(eye.position, eye.forward, out hit, viewRange, interactableObjects))
            {
                CheckObjectIsInteractable(hit.transform.gameObject);
            }
            else if (onVisionObject != null)
            {
                WhenNoObjectIsInteractable();
            }
        }
    }

    private void CheckObjectIsInteractable(GameObject obj)
    {
        GrabableObjectBehaviour grabableObject = obj.GetComponent<GrabableObjectBehaviour>();
        if (grabableObject != null)
        {
            onVisionObject = grabableObject.gameObject;
            UIEvents.Emit(new UpdateUIInteractionModeEvent(UIInteractionModes.GRAB));
            return;
        }
    }

    private void WhenNoObjectIsInteractable()
    {
        onVisionObject = null;
        UIEvents.Emit(new UpdateUIInteractionModeEvent(UIInteractionModes.EMPTY));
    }

    public bool InteractWithObject()
    {
        if (onVisionObject == null) return false;

        GrabableObjectBehaviour grabableObject = onVisionObject.GetComponent<GrabableObjectBehaviour>();
        if (grabableObject != null)
        {
            if (!isObjectGrabbed)
            {
                DeactivateVision(false);
                UIEvents.Emit(new UpdateUIInteractionModeEvent(UIInteractionModes.DROP));

                grabableObject.ActivateConstraint();
                isObjectGrabbed = true;

                return true;
            }
            else
            {
                grabableObject.DeactivateConstraint();
                isObjectGrabbed = false;

                ActivateVision(true);

                return false;
            }
        }

        return false;
    }

    public void ActivateVision(bool updateUI)
    {
        isVisionActive = true;
        if (updateUI) UIEvents.Emit(new UpdateUIInteractionModeEvent(UIInteractionModes.EMPTY));
    }

    public void DeactivateVision(bool updateUI)
    {
        isVisionActive = false;
        if (updateUI) UIEvents.Emit(new UpdateUIInteractionModeEvent(UIInteractionModes.EMPTY));
    }
}
