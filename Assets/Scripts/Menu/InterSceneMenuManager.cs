using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterSceneMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents buttonEvents;

    [SerializeField]
    private Utils.Scenes nextScene;

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
        if (data is PlayButtonEvent)
        {
            Utils.ChangeScene(nextScene);
        }
    }
}
