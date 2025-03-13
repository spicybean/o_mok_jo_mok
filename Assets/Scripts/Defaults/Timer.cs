using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image timer;
    public TMP_Text text;
    
    public delegate void TimerDelegate();
    public TimerDelegate OnTimerEndDelegate;
    public TimerDelegate OnTimerStartDelegate;

    public enum TimerType{Decrease, Increase}
    public TimerType timerType;
    public float timeLimit;
    private float currentTime;

    private bool _isPaused;
    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
        _isPaused = false;
    }

    void Update()
    {
        if(_isPaused) return;
        if (timerType == TimerType.Decrease)
        {
            currentTime -= timeLimit/ Time.deltaTime;
            if (currentTime <= 0)
            {
                OnTimerEndDelegate?.Invoke();
                _isPaused= true;
            }
        }
        if (timerType == TimerType.Increase)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeLimit)
            {
                OnTimerEndDelegate?.Invoke();
                _isPaused= true;
            }
            
        }
        timer.fillAmount = currentTime/timeLimit;
        text.text = ((int)currentTime).ToString();
    }
    void ResetTimer()
    {
        if (timerType == TimerType.Decrease)
        {
            currentTime = timeLimit;
        }
        else
        {
            currentTime = 0;
        }
        _isPaused= true;
    }
    
    
}



