using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraCollisionManager : MonoBehaviour
{
    [SerializeField]
    private DepthCameraVisionManager inVisionObjectsManager;

    private void OnTriggerEnter(Collider other)
    {
        OneWorldObjectMaterialUpdater updater = other.gameObject.GetComponent<OneWorldObjectMaterialUpdater>();
        if (updater != null) inVisionObjectsManager.ObjectEntered(updater);
    }

    private void OnTriggerExit(Collider other)
    {
        OneWorldObjectMaterialUpdater updater = other.gameObject.GetComponent<OneWorldObjectMaterialUpdater>();
        if (updater != null) inVisionObjectsManager.ObjectExited(updater);
    }
}
