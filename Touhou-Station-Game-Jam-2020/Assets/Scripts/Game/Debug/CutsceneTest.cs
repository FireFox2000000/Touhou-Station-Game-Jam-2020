using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

public class CutsceneTest : MonoBehaviour
{
    [SerializeField]
    BackgroundBlending m_cutsceneBackground;

    [SerializeField]
    Texture2D[] m_cutsceneBackgrounds;

    StateMachine m_stateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        m_cutsceneBackground.SetTexture(m_cutsceneBackgrounds[0]);
        m_stateMachine.currentState = new BackgroundTransitionState(m_cutsceneBackground, m_cutsceneBackgrounds[1], OnScene0Complete);
    }

    void OnScene0Complete()
    {
        m_stateMachine.currentState = new BackgroundTransitionState(m_cutsceneBackground, m_cutsceneBackgrounds[2], OnScene1Complete);
    }

    void OnScene1Complete()
    {
        m_stateMachine.currentState = new BackgroundTransitionState(m_cutsceneBackground, m_cutsceneBackgrounds[0], OnCutsceneComplete);
    }

    void OnCutsceneComplete()
    {
        m_stateMachine.currentState = null;
    }
}
