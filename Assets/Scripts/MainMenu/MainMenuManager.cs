using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
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
        if (data is PlayButtonEvent) StartGame();
        else if (data is OptionsButtonEvent) OpenOptions();
        else if (data is ExitButtonEvent) ExitGame();
    }

    private void StartGame()
    {
        Utils.ChangeScene(Utils.Scenes.GAME);
    }

    private void OpenOptions()
    {

    }

    private void ExitGame()
    {
        Utils.ExitGame();
    }
}
