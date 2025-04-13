using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraVisionManager : MonoBehaviour
{
    private List<OneWorldObjectMaterialUpdater> objectsInVision;
    private List<OneWorldObjectMaterialUpdater> objectsViewedInRecording;

    private bool isCameraRecording;

    private void Start()
    {
        objectsInVision = new List<OneWorldObjectMaterialUpdater>();
        objectsViewedInRecording = new List<OneWorldObjectMaterialUpdater>();
        isCameraRecording = false;
    }

    public void NotifyCameraIsRecording()
    {
        objectsInVision.ForEach((obj) => objectsViewedInRecording.Add(obj));
        isCameraRecording = true;
    }

    public void NotifyCameraIsReplaying()
    {
        isCameraRecording = false;
    }

    public void NotifyCameraIsDeactivated()
    {
        if (objectsViewedInRecording.Count > 0)
        {
            objectsViewedInRecording.ForEach((obj) => obj.ResetMap());
            objectsViewedInRecording.Clear();
        }
    }

    public void NotifyObjectsRecord(float timeStamp, Vector3 camPosition)
    {
        objectsInVision.ForEach((obj) => obj.RecordState(timeStamp, camPosition));
    }

    public void NotifyObjectsReplay(float timeStamp)
    {
        objectsViewedInRecording.ForEach((obj) => obj.ReplayState(timeStamp));
    }

    public void ObjectEntered(OneWorldObjectMaterialUpdater obj)
    {
        if (!objectsInVision.Contains(obj))
        {
            objectsInVision.Add(obj);
            if (isCameraRecording) objectsViewedInRecording.Add(obj);
        }
    }

    public void ObjectExited(OneWorldObjectMaterialUpdater obj)
    {
        if (objectsInVision.Contains(obj))
        {
            objectsInVision.Remove(obj);
        }
    }
}
