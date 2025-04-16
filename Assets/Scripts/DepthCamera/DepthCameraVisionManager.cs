using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraVisionManager : MonoBehaviour
{
    private List<CameraAwareObject> objectsInVision;
    private List<CameraAwareObject> objectsViewedInRecording;

    private bool isCameraRecording;

    private void Start()
    {
        objectsInVision = new List<CameraAwareObject>();
        objectsViewedInRecording = new List<CameraAwareObject>();
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

    public void ObjectEntered(CameraAwareObject obj)
    {
        if (!objectsInVision.Contains(obj))
        {
            objectsInVision.Add(obj);
            if (isCameraRecording) objectsViewedInRecording.Add(obj);
        }
    }

    public void ObjectExited(CameraAwareObject obj)
    {
        if (objectsInVision.Contains(obj))
        {
            objectsInVision.Remove(obj);
        }
    }
}
