using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

class BackgroundTransitionState : StateMachine.IState
{
    Texture2D m_newTex;
    BackgroundBlending m_backgroundManager;
    BackgroundBlending.OnCompleteFn m_onCompleteCallback;

    public BackgroundTransitionState(BackgroundBlending backgroundManager, Texture2D newTex, BackgroundBlending.OnCompleteFn onCompleteCallback)
    {
        m_backgroundManager = backgroundManager;
        m_newTex = newTex;
        m_onCompleteCallback = onCompleteCallback;
    }

    public void Enter()
    {
        m_backgroundManager.KickTransition(m_newTex, m_onCompleteCallback);
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
