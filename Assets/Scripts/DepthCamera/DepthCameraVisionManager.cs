using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraVisionManager : MonoBehaviour
{
    private List<OneWorldObjectMaterialUpdater> objectsInVision;

    private bool isCameraActive;

    private void Start()
    {
        objectsInVision = new List<OneWorldObjectMaterialUpdater>();
        isCameraActive = false;
    }

    public void NotifyCameraIsActivated()
    {
        objectsInVision.ForEach((obj) => obj.SetObjectActive());
        isCameraActive = true;
    }

    public void NotifyCameraIsDeactivated()
    {
        objectsInVision.ForEach((obj) => obj.SetObjectInactive());
        isCameraActive = false;
    }

    public void ObjectEntered(OneWorldObjectMaterialUpdater obj)
    {
        if (!objectsInVision.Contains(obj))
        {
            objectsInVision.Add(obj);
            if (isCameraActive) obj.SetObjectActive();
        }
    }

    public void ObjectExited(OneWorldObjectMaterialUpdater obj)
    {
        if (objectsInVision.Contains(obj))
        {
            objectsInVision.Remove(obj);
            if (isCameraActive) obj.SetObjectInactive();
        }
    }
}
