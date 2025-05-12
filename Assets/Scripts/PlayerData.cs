using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    private Utils.ControlScheme activeScheme;

    public void SaveActiveScheme(Utils.ControlScheme scheme)
    {
        activeScheme = scheme;
    }

    public Utils.ControlScheme GetActiveScheme()
    {
        return activeScheme;
    }
}
