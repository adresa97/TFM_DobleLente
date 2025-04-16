using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraCollisionManager : MonoBehaviour
{
    [SerializeField]
    private DepthCameraVisionManager inVisionObjectsManager;

    private void OnTriggerEnter(Collider other)
    {
        CameraAwareObject updater = other.gameObject.GetComponent<CameraAwareObject>();
        if (updater != null) inVisionObjectsManager.ObjectEntered(updater);
    }

    private void OnTriggerExit(Collider other)
    {
        CameraAwareObject updater = other.gameObject.GetComponent<CameraAwareObject>();
        if (updater != null) inVisionObjectsManager.ObjectExited(updater);
    }
}
