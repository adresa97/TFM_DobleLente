using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseModeManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents buttonEvents;

    private void OnEnable()
    {
        buttonEvents.AddListener(ButtonEventsCallback);
    }

    private void OnDisable()
    {
        buttonEvents.RemoveListener(ButtonEventsCallback);
    }

    private void ButtonEventsCallback(object data)
    {
        if (data is ToMainMenuButtonEvent) Utils.ChangeScene(Utils.Scenes.MENU);
        else if (data is ExitButtonEvent) Utils.ExitGame();
    }
}
