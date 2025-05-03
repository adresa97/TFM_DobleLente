using Codice.CM.WorkspaceServer.Tree.GameUI.Checkin.Updater;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisionManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents playerToCameraEvents;

    [SerializeField]
    private Transform eye;

    [SerializeField]
    private float viewRange;

    private GameObject onVisionObject;
    private LayerMask interactableObjects;

    [SerializeField]
    private Transform grabPoint;

    private bool isVisionActive;
    private bool isObjectGrabbed;

    private void Start()
    {
        interactableObjects = LayerMask.GetMask("BothWorlds", "RealWorld", "FromOtherWorld", "UniqueItems");

        onVisionObject = null;

        isVisionActive = true;
        isObjectGrabbed = false;
    }

    private void FixedUpdate()
    {
        if (isVisionActive && !isObjectGrabbed)
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
            playerToCameraEvents.Emit(new GrabableObjectOnVisionEvent());
            return;
        }

        ManualSwitchLogic interactableObject = obj.GetComponent<ManualSwitchLogic>();
        if (interactableObject != null)
        {
            onVisionObject = interactableObject.gameObject;
            playerToCameraEvents.Emit(new InteractableObjectOnVisionEvent());
            return;
        }
    }

    private void WhenNoObjectIsInteractable()
    {
        onVisionObject = null;
        playerToCameraEvents.Emit(new NoObjectOnVisionEvent());
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
                playerToCameraEvents.Emit(new ObjectGrabbedEvent());

                grabableObject.ActivateConstraint(grabPoint);
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

        ManualSwitchLogic interactableObject = onVisionObject.GetComponent<ManualSwitchLogic>();
        if (interactableObject != null)
        {
            if (!isObjectGrabbed)
            {
                interactableObject.Interact();
            }
        }

        return false;
    }

    public void ActivateVision(bool updateUI)
    {
        isVisionActive = true;
        if (updateUI) playerToCameraEvents.Emit(new NoObjectOnVisionEvent());
    }

    public void DeactivateVision(bool updateUI)
    {
        isVisionActive = false;
        if (updateUI) playerToCameraEvents.Emit(new NoObjectOnVisionEvent());
    }
}
