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

    [SerializeField]
    protected AudioSource soundPlayer;

    [SerializeField]
    private MeshRenderer colorRenderer;
    private MaterialPropertyBlock colorProperty;

    [SerializeField]
    private Color offColor;

    [SerializeField]
    private Color onColor;

    [SerializeField]
    private bool isReal;

    protected bool isPressed;
    protected bool isActive;

    protected virtual void Start()
    {
        isPressed = false;
        isActive = false;

        colorProperty = new MaterialPropertyBlock();

        SetOffColor();
    }

    protected void SetOnColor()
    {
        float realAlpha = colorRenderer.material.GetColor("_RealWorldAlbedo").a;

        colorProperty.Clear();
        if (realAlpha != 0f)
        {
            colorProperty.SetColor("_RealWorldAlbedo", onColor);
        }
        else
        {
            colorProperty.SetColor("_OtherWorldAlbedo", onColor);
        }

        colorRenderer.SetPropertyBlock(colorProperty);
    }

    protected void SetOffColor()
    {
        float realAlpha = colorRenderer.material.GetColor("_RealWorldAlbedo").a;

        colorProperty.Clear();
        if (realAlpha != 0f)
        {
            colorProperty.SetColor("_RealWorldAlbedo", offColor);
        }
        else
        {
            colorProperty.SetColor("_OtherWorldAlbedo", offColor);
        }

        colorRenderer.SetPropertyBlock(colorProperty);
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
