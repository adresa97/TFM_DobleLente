using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameEvents interactionEvents;

    [SerializeField]
    private int endingFrequency;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        interactionEvents.AddListener(InteractionEventsCallback);
    }

    private void OnDisable()
    {
        interactionEvents.RemoveListener(InteractionEventsCallback);
    }

    private void InteractionEventsCallback(object data)
    {
        if (data is ActivateSignalEvent)
        {
            if ((data as ActivateSignalEvent).signal == endingFrequency)
            {
                Utils.ChangeScene(Utils.Scenes.MENU);
            }
        }
    }
}
