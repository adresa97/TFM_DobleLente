using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeControlIcon : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private GameEvents inputEvents;

    [SerializeField]
    private Image imageUI;

    [SerializeField]
    private Sprite controllerIcon;

    [SerializeField]
    private Sprite keyboardMouseIcon;

    private void OnEnable()
    {
        ChangeIcon(playerData.GetActiveScheme());
        inputEvents.AddListener(InputEventsCallback);
    }

    private void OnDisable()
    {
        inputEvents.RemoveListener(InputEventsCallback);
    }

    private void InputEventsCallback(object data)
    {
        if (data is InputSchemeChanged)
        {
            ChangeIcon((data as InputSchemeChanged).scheme);
        }
    }

    private void ChangeIcon(Utils.ControlScheme scheme)
    {
        if (scheme == Utils.ControlScheme.KEYBOARD)
        {
            imageUI.sprite = keyboardMouseIcon;
        }
        else if (scheme == Utils.ControlScheme.GAMEPAD)
        {
            imageUI.sprite = controllerIcon;
        }
    }
}
