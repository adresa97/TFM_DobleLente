using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DepthCameraReplayer : MonoBehaviour
{
    private struct CameraState
    {
        private Vector3 position;
        private Vector3 forward;

        public CameraState(Vector3 position, Vector3 forward)
        {
            this.position = position;
            this.forward = forward;
        }

        public Vector3 GetPosition() { return position; }
        public Vector3 GetForward() { return forward; }
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
        CameraState state = new CameraState(depthCamera.position, depthCamera.forward);
        cameraStatesMap[timeStamp] = state;
    }

    public void ReplayState(float timeStamp)
    {
        if (cameraStatesMap.ContainsKey(timeStamp))
        {
            depthCamera.position = cameraStatesMap[timeStamp].GetPosition();
            depthCamera.forward = cameraStatesMap[timeStamp].GetForward();
        }
    }

    public void ResetMap()
    {
        cameraStatesMap.Clear();
    }
}
