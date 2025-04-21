using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text container;
    private int counter;
    private int totalTime;
    private bool isCounting;

    private void Start()
    {
        totalTime = 15;
    }

    public void InitTimer()
    {
        counter = 0;
        isCounting = true;
    }

    public void IncrementTimer()
    {
        if (isCounting && counter < totalTime)
        {
            counter++;
            DrawTimer();
        }
    }

    public void ResetTimer()
    {
        if (isCounting)
        {
            EraseTimer();
            counter = 0;
            isCounting = false;
        }
    }

    private void DrawTimer()
    {
        string formattedTime = "00:" + counter.ToString("00");
        container.text = formattedTime;
    }

    private void EraseTimer()
    {
        container.text = "";
    }
}