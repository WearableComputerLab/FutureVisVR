using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [Header("Component")]
    public TextMeshProUGUI timerText;
    [Header("Timer Settings")]
    public static float currentTime;
    // public bool countDown;

    // Update is called once per frame
    void Update()
    {
        // currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("0.0");
    }
}
