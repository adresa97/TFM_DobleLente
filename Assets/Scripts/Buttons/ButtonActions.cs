using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    [SerializeField]
    private GameEvents buttonEvents;

    public void OnPlayGameButton()
    {
        buttonEvents.Emit(new PlayButtonEvent());
    }

    public void OnOpenOptionsButton()
    {
        buttonEvents.Emit(new OptionsButtonEvent());
    }

    public void OnExitButton()
    {
        buttonEvents.Emit(new ExitButtonEvent());
    }
}
