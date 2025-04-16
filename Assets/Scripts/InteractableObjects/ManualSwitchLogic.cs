using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSwitchLogic : SwitchLogic
{
    public void PressSwitch()
    {
        if (!isPressed)
        {
            isPressed = true;

            if (HasBrokenCables()) StartCoroutine(CheckWhilePressed());
            else SendActivateSignal();
        }
    }

    public void ReleaseSwitch()
    {
        if (isPressed)
        {
            isPressed = false;

            if (!HasBrokenCables()) SendDeactivateSignal();
        }
    }
}
