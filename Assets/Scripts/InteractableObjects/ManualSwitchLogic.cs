using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSwitchLogic : SwitchLogic
{
    public void Interact()
    {
        if (isPressed)
        {
            ReleaseSwitch();
        }
        else
        {
            PressSwitch();
        }
    }

    public void PressSwitch()
    {
        if (!isPressed)
        {
            isPressed = true;
            SetOnColor();

            soundPlayer.Play();

            if (HasBrokenCables()) StartCoroutine(CheckWhilePressed());
            else SendActivateSignal();
        }
    }

    public void ReleaseSwitch()
    {
        if (isPressed)
        {
            isPressed = false;
            SetOffColor();

            if (!HasBrokenCables()) SendDeactivateSignal();
        }
    }
}
