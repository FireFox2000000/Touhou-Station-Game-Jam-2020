using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

public class FrontendStateManager : MonoBehaviour
{
    [SerializeField]
    FrontendMenuBase m_initialState;

    StateMachine m_stateMachine = new StateMachine();

    void Start()
    {
        m_initialState.Init(m_stateMachine);
        m_stateMachine.currentState = m_initialState;
    }

    void Update()
    {
        m_stateMachine.Update();
    }
}
