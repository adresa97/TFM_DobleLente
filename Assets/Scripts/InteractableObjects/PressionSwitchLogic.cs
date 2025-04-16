using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressionSwitchLogic : SwitchLogic
{
    private List<GameObject> objectsPressing;

    protected override void Start()
    {
        base.Start();

        objectsPressing = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CanActivateSwitches") || other.gameObject.CompareTag("Player"))
        {
            objectsPressing.Add(other.gameObject);
            if (!isPressed)
            {
                isPressed = true;

                if (HasBrokenCables()) StartCoroutine(CheckWhilePressed());
                else SendActivateSignal();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsPressing.Contains(other.gameObject))
        {
            objectsPressing.Remove(other.gameObject);

            if (objectsPressing.Count == 0 && isPressed)
            {
                isPressed = false;

                if (!HasBrokenCables()) SendDeactivateSignal();
            }
        }
    }
}
