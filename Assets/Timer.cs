using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
    public int setTime;
    private int currentTime;
    private TextMeshProUGUI timer;
    public bool isRunning;
    void Start()
    {
        this.currentTime = setTime;
        timer = gameObject.GetComponent<TextMeshProUGUI>();
        timer.text = "Time untill attack commences: " + this.currentTime;
        StartCoroutine(StartTimer());
    }

    private void Reset(int setTime = 30)
    {
        currentTime = setTime;
    }

    public IEnumerator StartTimer()
    {
        Reset();
        isRunning = true;
        while ( currentTime != 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            timer.text = "Time untill attack commences: " + this.currentTime;
        }
        isRunning = false;
        StopCoroutine(StartTimer());
    }

}
