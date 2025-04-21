using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayMovingInteractee : MonoBehaviour
{
    private enum DIRECTION_WAY { BACKWARD = -1, STOP = 0, FORWARD = 1 };

    [SerializeField]
    private GameEvents interactionEvents;

    [SerializeField]
    private int signal;

    [SerializeField]
    private Transform initialPoint;
    private bool isOnInitial;

    [SerializeField]
    private Transform targetPoint;
    private bool isOnTarget;

    [SerializeField]
    private DIRECTION_WAY directionWay;
    private Vector3 moveDirection;

    [SerializeField]
    private int moveTime;
    private float speed;

    private void Start()
    {
        transform.position = initialPoint.position;

        moveDirection = (targetPoint.position - initialPoint.position).normalized;
        speed = (targetPoint.position - initialPoint.position).magnitude / moveTime;

        isOnInitial = directionWay == DIRECTION_WAY.STOP ? true : false;
        isOnTarget = false;
    }

    private void OnEnable()
    {
        interactionEvents.AddListener(InteractionEventsCallback);
    }

    private void OnDisable()
    {
        interactionEvents.RemoveListener(InteractionEventsCallback);
    }

    private void InteractionEventsCallback(object data)
    {
        if (data is ActivateSignalEvent)
        {
            TryToActivate((data as ActivateSignalEvent).signal);
        }
        else if (data is DeactivateSignalEvent)
        {
            TryToDeactivate((data as DeactivateSignalEvent).signal);
        }
    }

    private void FixedUpdate()
    {
        if (directionWay == DIRECTION_WAY.FORWARD && !isOnTarget)
        {
            transform.position = transform.position + (moveDirection * speed * (int)directionWay);
            if (transform.position == targetPoint.position) isOnTarget = true;
        }
        else if (directionWay == DIRECTION_WAY.BACKWARD && !isOnInitial)
        {
            transform.position = transform.position + (moveDirection * speed * (int)directionWay);
            if (transform.position == initialPoint.position) isOnInitial = true;
        }
    }

    private void TryToActivate(int inSignal)
    {
        if (inSignal == signal)
        {
            directionWay = DIRECTION_WAY.FORWARD;
            isOnInitial = false;
        }
    }

    private void TryToDeactivate(int inSignal)
    {
        if (inSignal == signal)
        {
            directionWay = DIRECTION_WAY.BACKWARD;
            isOnTarget = false;
        }
    }
}
