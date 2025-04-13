using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DepthCameraReplayer : MonoBehaviour
{
    private struct CameraState
    {
        private Vector3 position;
        private Quaternion rotation;

        public CameraState(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector3 GetPosition() { return position; }
        public Quaternion GetRotation() { return rotation; }
    }

    [SerializeField]
    private Transform depthCamera;

    private Dictionary<float, CameraState> cameraStatesMap;

    private void Start()
    {
        cameraStatesMap = new Dictionary<float, CameraState>();
        cameraStatesMap.Clear();
    }

    public void RecordState(float timeStamp)
    {
        CameraState state = new CameraState(depthCamera.position, depthCamera.rotation);
        cameraStatesMap[timeStamp] = state;
    }

    public void ReplayState(float timeStamp)
    {
        if (cameraStatesMap.ContainsKey(timeStamp))
        {
            depthCamera.position = cameraStatesMap[timeStamp].GetPosition();
            depthCamera.rotation = cameraStatesMap[timeStamp].GetRotation();
        }
    }

    public void ResetMap()
    {
        cameraStatesMap.Clear();
    }
}
