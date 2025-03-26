using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWorldObjectMaterialUpdater : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer objectRenderer;

    [SerializeField]
    private bool isRealWorld;

    private MaterialPropertyBlock dualWorldMaterial;

    private void Start()
    {
        dualWorldMaterial = new MaterialPropertyBlock();
        SetObjectInactive();
    }

    public void SetObjectActive()
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
    }

    public void SetObjectInactive()
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
    }
}
