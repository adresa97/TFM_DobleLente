using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecordSignal : MonoBehaviour
{
    [SerializeField]
    private GameObject recordSymbol;

    private void Start()
    {
        recordSymbol.SetActive(false);
    }

    public void StartRecordingSymbols()
    {
        recordSymbol.SetActive(true);
    }

    public void StopRecordingSymbols()
    {
        recordSymbol.SetActive(false);
    }
}
