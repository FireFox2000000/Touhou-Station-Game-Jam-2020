using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;
using UnityEngine.EventSystems;

public abstract class FrontendMenuBase : MonoBehaviour, StateMachine.IState
{
    [SerializeField]
    GameObject m_firstSelectableObject;

    StateMachine m_stateMachine;
    bool m_enterAwake = false;
    GameObject m_lastKnownSelectedObject = null;
    static Vector3 m_lastKnownMousePos;
    EventSystem m_eventSystem;

    public void Init(StateMachine sm)
    {
        m_stateMachine = sm;
    }

    void Awake()
    {
        if (!m_enterAwake)
            gameObject.SetActive(false);        // When first playing the scene, helps speed up debugging

        m_eventSystem = EventSystem.current;
    }

    public void Enter()
    {
        Debug.Assert(m_stateMachine != null, "Must call Init() method before changing state");

        m_enterAwake = true;
        gameObject.SetActive(true);
        SelectSelf();
        m_lastKnownMousePos = Input.mousePosition;
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        m_stateMachine = null;
    }

    public void Update()
    {
        GameObject selected = m_eventSystem.currentSelectedGameObject;

        // Monitor the last known selected element of our menu
        if (selected)
        {
            if (selected.transform.IsChildOf(this.transform))
            {
                m_lastKnownSelectedObject = selected;
            }
        }

        // Detect any controller/keyboard input, then set a selected object so we can actually navigate the menus
        if (!selected)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
            {
                SelectSelf();
            }
        }

        // If the mouse moves deselect the current object, user prefers mouse controls
        else
        {
            if (Input.mousePosition != m_lastKnownMousePos)
            {
                m_eventSystem.SetSelectedGameObject(null);
            }
        }

        m_lastKnownMousePos = Input.mousePosition;
    }

    public void ChangeState(FrontendMenuBase nextState)
    {
        nextState.Init(m_stateMachine);
        m_stateMachine.currentState = nextState;
    }

    GameObject DefaultSelectableObject
    {
        get
        {
            if (m_lastKnownSelectedObject)
            {
                return m_lastKnownSelectedObject;
            }
            else
            {
                return m_firstSelectableObject;
            }
        }
    }

    void SelectSelf()
    {
        EventSystem.current.SetSelectedGameObject(DefaultSelectableObject);
    }
}
