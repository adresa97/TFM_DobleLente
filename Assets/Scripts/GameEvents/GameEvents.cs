using System;
using System.Collections.Generic;
using UnityEngine;

// Based on GameEvents script created by Ryan Hipple and hosted at https://github.com/roboryantron/Unite2017/blob/master/Assets/Code/Events/GameEvent.cs
// Modified callback type in order to send actions

[CreateAssetMenu]
public class GameEvents : ScriptableObject
{
    // List of listeners that this event will notify if it is raised.
    private readonly IList<Action<System.Object>> eventListeners =
        new List<Action<System.Object>>();

    public void Emit(System.Object data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].Invoke(data);
        }
    }

    public void AddListener(Action<System.Object> listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void RemoveListener(Action<System.Object> listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}