using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionAreaTrigger : MonoBehaviour
{
    [SerializeField]
    private GameEvents interactionEvents;

    [SerializeField]
    private int signalFrequency;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionEvents.Emit(new ActivateSignalEvent(signalFrequency));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionEvents.Emit(new DeactivateSignalEvent(signalFrequency));
        }
    }
}
