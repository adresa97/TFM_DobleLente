using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamerasManager : MonoBehaviour
{
    [SerializeField]
    private Camera realCamera;

    [SerializeField]
    private LayerMask commonCulling;

    [SerializeField]
    private Color previewBackgroundColor;

    [SerializeField]
    private float previewFarPlane;

    private int realCulling;
    private float realFarPLane;
    private int previewCulling;

    private void Awake()
    {
        realCulling = commonCulling | (1 << LayerMask.NameToLayer("RealWorld"));
        previewCulling = commonCulling | (1 << LayerMask.NameToLayer("OtherWorld"));

        realFarPLane = realCamera.farClipPlane;
    }

    public void SetRealCamera()
    {
        realCamera.cullingMask = realCulling;
        realCamera.clearFlags = CameraClearFlags.Skybox;
        realCamera.farClipPlane = realFarPLane;
        RenderSettings.fog = false;
    }

    public void SetPreviewCamera()
    {
        realCamera.cullingMask = previewCulling;
        realCamera.backgroundColor = previewBackgroundColor;
        realCamera.clearFlags = CameraClearFlags.SolidColor;
        realCamera.farClipPlane = previewFarPlane;
        RenderSettings.fog = true;
    }
}
