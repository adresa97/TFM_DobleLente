using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DepthCameraUpdater : MonoBehaviour
{
    [SerializeField]
    private RenderTexture depthMap;

    private Camera thisCamera;
    private Matrix4x4 camMatrix;

    private float errorFactor = 0.1f;

    void Start()
    {
        thisCamera = GetComponent<Camera>();

        // Initialize static global shader variables
        Shader.SetGlobalFloat("_DepthCameraNearPlane", thisCamera.nearClipPlane);
        Shader.SetGlobalFloat("_DepthCameraFarPlane", thisCamera.farClipPlane);
        Shader.SetGlobalFloat("_DepthCameraFOV", thisCamera.fieldOfView);

        Shader.SetGlobalFloat("_DepthCameraErrorFactor", errorFactor);

        DeactivateCamera();
        DeactivatePreviewCamera();

        // Calculate this camera current view matrix and intialize its global shader variables
        CalculeCameraMatrix();
        UpdateValues();
    }

    void Update()
    {
        // Each frame calculate view matrix and update its global shader variables (matrix and texture)
        CalculeCameraMatrix();
        UpdateValues();
    }

    void CalculeCameraMatrix()
    {
        // Get camera matrix and reverse if not reversed by default
        camMatrix = thisCamera.worldToCameraMatrix;
        if(!SystemInfo.usesReversedZBuffer)
        {
            camMatrix.m20 = -camMatrix.m20;
            camMatrix.m21 = -camMatrix.m21;
            camMatrix.m22 = -camMatrix.m22;
            camMatrix.m23 = -camMatrix.m23;
        }
    }

    void UpdateValues()
    {
        // Update camera depth texture and view matrix global shader variables
        Shader.SetGlobalTexture("_DepthCameraMap", depthMap);
        Shader.SetGlobalMatrix("_DepthCameraView", camMatrix);
    }

    public void ActivateCamera()
    {
        // Set global shader active variable to 1 (true)
        Shader.SetGlobalFloat("_IsDepthCameraActive", 1);
    }

    public void DeactivateCamera()
    {
        // Set global shader active variable to 0 (false)
        Shader.SetGlobalFloat("_IsDepthCameraActive", 0);
    }

    public void ActivatePreviewCamera()
    {
        // Set global shader preview variable to 1 (true)
        Shader.SetGlobalFloat("_IsDepthCameraPreview", 1);
    }

    public void DeactivatePreviewCamera()
    {
        // Set global shader preview variable to 0 (false)
        Shader.SetGlobalFloat("_IsDepthCameraPreview", 0);
    }
}
