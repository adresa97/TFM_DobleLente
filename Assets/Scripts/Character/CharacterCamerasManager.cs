using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamerasManager : MonoBehaviour
{
    [SerializeField]
    private Camera realCamera;

    [SerializeField]
    private LayerMask commonCulling;

    private int realCulling;
    private int previewCulling;

    private void Awake()
    {
        realCulling = commonCulling | (1 << LayerMask.NameToLayer("RealWorld"));
        previewCulling = commonCulling | (1 << LayerMask.NameToLayer("OtherWorld"));
    }

    public void SetRealCamera()
    {
        realCamera.cullingMask = realCulling;
        RenderSettings.fog = false;
    }

    public void SetPreviewCamera()
    {
        realCamera.cullingMask = previewCulling;
        RenderSettings.fog = true;
    }
}
