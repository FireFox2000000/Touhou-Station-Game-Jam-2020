using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

public abstract class FrontendMenuBase : MonoBehaviour, StateMachine.IState
{
    StateMachine m_stateMachine;

    public void Init(StateMachine sm)
    {
        m_stateMachine = sm;
    }

    public void Enter()
    {
        Debug.Assert(m_stateMachine != null, "Must call Init() method before changing state");

        gameObject.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        m_stateMachine = null;
    }

    public void Update()
    {
    }

    public void ChangeState(FrontendMenuBase nextState)
    {
        nextState.Init(m_stateMachine);
        m_stateMachine.currentState = nextState;
    }
}
