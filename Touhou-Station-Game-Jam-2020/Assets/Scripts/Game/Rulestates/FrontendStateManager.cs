using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

[UnitySingleton(UnitySingletonAttribute.Type.ExistsInScene, true)]
public class FrontendStateManager : UnitySingleton<FrontendStateManager>
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

    public T GetUIElement<T>() where T : MonoBehaviour
    {
        return gameObject.GetComponentInChildren<T>(true);
    }
}
