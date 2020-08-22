using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

public class DelayState : StateMachine.IState
{
    public delegate void OnCompleteFn();

    float m_totalDelayTime;
    float m_startTime;
    OnCompleteFn m_onCompleteCallback;

    public DelayState(float totalDelayTime, OnCompleteFn onCompleteCallback)
    {
        m_totalDelayTime = totalDelayTime;
        m_onCompleteCallback = onCompleteCallback;
    }

    public void Enter()
    {
        m_startTime = Time.time;
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (Time.time - m_startTime > m_totalDelayTime)
        {
            m_onCompleteCallback();
        }
    }
}