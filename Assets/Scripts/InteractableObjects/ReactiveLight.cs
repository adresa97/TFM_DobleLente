using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveLight : MonoBehaviour
{
    [SerializeField]
    private GameEvents interactionEvents;

    [SerializeField]
    private int frequency;

    [SerializeField]
    private Light lightObject;

    [SerializeField]
    private Color activeColor;

    [SerializeField]
    private Color inactiveColor;

    private void Start()
    {
        lightObject.color = inactiveColor;
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
            if ((data as ActivateSignalEvent).signal == frequency)
            {
                lightObject.color = activeColor;
            }
        }
        else if (data is DeactivateSignalEvent)
        {
            if ((data as DeactivateSignalEvent).signal == frequency)
            {
                lightObject.color = inactiveColor;
            }
        }
    }
}
