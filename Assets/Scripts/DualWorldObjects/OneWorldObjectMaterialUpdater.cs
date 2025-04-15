using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class OneWorldObjectMaterialUpdater : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer objectRenderer;

    [SerializeField]
    private bool isRealWorld;

    [SerializeField]
    private Transform[] visionPoints;
    private Dictionary<float, bool> isActiveMap;

    private bool isActive;

    private LayerMask raycastLayerMask;

    private MaterialPropertyBlock dualWorldMaterial;

    private void Start()
    {
        dualWorldMaterial = new MaterialPropertyBlock();
        isActiveMap = new Dictionary<float, bool>();
        isActiveMap.Clear();

        isActive = false;

        raycastLayerMask =~ LayerMask.GetMask("Player");

        SetObjectInactive();
    }

    public void RecordState(float timeStamp, Vector3 camPosition)
    {
        isActiveMap[timeStamp] = CheckVisibility(camPosition);
    }

    /*
    private bool CheckVisibility(Vector3 camPosition)
    {
        Vector3 distance = camPosition - transform.position;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, distance.normalized, out hit, distance.magnitude, raycastLayerMask))
        {
            if (hit.collider.tag == "DepthCameraRaycast") return true;
        }

        return false;
    }
    */

    private bool CheckVisibility(Vector3 camPosition)
    {
        for (int i = 0; i < visionPoints.Length; i++)
        {
            Vector3 distance = camPosition - visionPoints[i].position;

            RaycastHit hit;
            if (Physics.Raycast(visionPoints[i].position, distance.normalized, out hit, distance.magnitude, raycastLayerMask))
            {
                if (hit.collider.tag == "DepthCameraRaycast") return true;
            }
        }

        return false;
    }

    public void ReplayState(float timeStamp)
    {
        if(isActiveMap.ContainsKey(timeStamp))
        {
            if (isActiveMap[timeStamp])
            {
                if (!isActive) SetObjectActive();
            }
            else
            {
                if (isActive) SetObjectInactive();
            }
        }
        else
        {
            if (isActive) SetObjectInactive();
        }
    }

    public void ResetMap()
    {
        if (isActive) SetObjectInactive();
        isActiveMap.Clear();
    }

    private void SetObjectActive()
    {
        dualWorldMaterial.SetFloat("_IsObjectActive", 1);
        objectRenderer.SetPropertyBlock(dualWorldMaterial);

        if (isRealWorld)
        {
            gameObject.layer = LayerMask.NameToLayer("FromRealWorld");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("FromOtherWorld");
        }

        isActive = true;
    }

    private void SetObjectInactive()
    {
        dualWorldMaterial.SetFloat("_IsObjectActive", 0);
        objectRenderer.SetPropertyBlock(dualWorldMaterial);

        if (isRealWorld)
        {
            gameObject.layer = LayerMask.NameToLayer("RealWorld");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("OtherWorld");
        }

        isActive = false;
    }
}
