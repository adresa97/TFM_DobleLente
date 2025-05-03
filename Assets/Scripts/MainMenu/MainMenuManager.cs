using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents buttonEvents;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

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
        //Utils.ChangeScene(Utils.Scenes.TEST);
    }

    private void OpenOptions()
    {

    }

    private void ExitGame()
    {
        Utils.ExitGame();
    }
}
