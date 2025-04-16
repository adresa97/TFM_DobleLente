using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SwitchLogic : MonoBehaviour
{
    [SerializeField]
    private GameEvents interactionEvents;

    [SerializeField]
    private int signalFrequency;

    [SerializeField]
    private CableStatusChecker[] cableSegments;

    protected bool isPressed;
    protected bool isActive;

    protected virtual void Start()
    {
        isPressed = false;
        isActive = false;
    }

    private bool IsCablesWorking()
    {
        for (int i = 0; i < cableSegments.Count(); i++)
        {
            if (!cableSegments[i].IsCableWorking()) return false;
        }

        return true;
    }

    protected bool HasBrokenCables()
    {
        return cableSegments.Count() > 0;
    }

    protected void SendActivateSignal()
    {
        interactionEvents.Emit(new ActivateSignalEvent(signalFrequency));
    }

    protected void SendDeactivateSignal()
    {
        interactionEvents.Emit(new DeactivateSignalEvent(signalFrequency));
    }

    protected IEnumerator CheckWhilePressed()
    {
        while (isPressed)
        {
            if (IsCablesWorking())
            {
                if (!isActive)
                {
                    SendActivateSignal();
                    isActive = true;
                }
            }
            else if (isActive)
            {
                SendDeactivateSignal();
                isActive = false;
            }

            yield return new WaitForSecondsRealtime(1.0f);
        }

        if (isActive)
        {
            SendDeactivateSignal();
            isActive = false;
        }
    }
}
