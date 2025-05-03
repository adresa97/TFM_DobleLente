using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayMovingInteractee : MonoBehaviour
{
    private enum DIRECTION_WAY { BACKWARD = -1, STOP = 0, FORWARD = 1 };

    [SerializeField]
    private GameEvents interactionEvents;

    [SerializeField]
    private int[] signals;
    private bool[] activedSignals;

    [SerializeField]
    private Transform initialPoint;
    private bool isOnInitial;

    [SerializeField]
    private Transform targetPoint;
    private bool isOnTarget;

    [SerializeField]
    private DIRECTION_WAY directionWay;

    private float movingProgress;

    [SerializeField]
    private int moveTime;
    private float speed;

    private void Start()
    {
        transform.position = initialPoint.position;

        movingProgress = 0;
        speed = 1.0f / moveTime;

        isOnInitial = directionWay == DIRECTION_WAY.STOP ? true : false;
        isOnTarget = false;

        activedSignals = new bool[signals.Length];
        for (int i = 0; i < signals.Length; i++)
        {
            activedSignals[i] = false;
        }
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
            movingProgress += speed;
            if (movingProgress >= 1)
            {
                movingProgress = 1;
                isOnTarget = true;
            }

            transform.position = initialPoint.position + (targetPoint.position - initialPoint.position) * movingProgress;
        }
        else if (directionWay == DIRECTION_WAY.BACKWARD && !isOnInitial)
        {
            movingProgress -= speed;
            if (movingProgress <= 0)
            {
                movingProgress = 0;
                isOnInitial = true;
            }

            transform.position = Vector3.Lerp(initialPoint.position, targetPoint.position, movingProgress);
        }
    }

    private void TryToActivate(int inSignal)
    {
        bool isAllActive = true;
        for (int i = 0; i < signals.Length; i++)
        {
            if (inSignal == signals[i])
            {
                activedSignals[i] = true;
            }

            if (!activedSignals[i]) isAllActive = false;
        }

        if (isAllActive)
        {
            directionWay = DIRECTION_WAY.FORWARD;
            isOnInitial = false;
        }
    }

    private void TryToDeactivate(int inSignal)
    {
        bool isAllActive = true;
        for (int i = 0; i < signals.Length; i++)
        {
            if (inSignal == signals[i])
            {
                activedSignals[i] = true;
            }

            if (!activedSignals[i]) isAllActive = false;
        }

        if (isAllActive)
        {
            directionWay = DIRECTION_WAY.BACKWARD;
            isOnTarget = false;
        }
    }
}
