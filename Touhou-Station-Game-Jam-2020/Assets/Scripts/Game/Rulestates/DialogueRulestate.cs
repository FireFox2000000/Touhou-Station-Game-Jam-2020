using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

public class DialogueRulestate : StateMachine.IState
{
    public delegate void OnFinishedFn();

    StateMachine m_stateMachine;
    DialogueScript m_script;
    IDialogueUI m_uiMananger;
    int m_currentScriptSequenceIndex = -1;
    OnFinishedFn m_onFinishedCallback;

    public DialogueRulestate(
        StateMachine stateMachine, 
        DialogueScript script, 
        IDialogueUI uiMananger, 
        OnFinishedFn onFinishedCallback)
    {
        m_stateMachine = stateMachine;
        m_script = script;
        m_uiMananger = uiMananger;
        m_onFinishedCallback = onFinishedCallback;
    }

    public void Enter()
    {
        m_uiMananger.Open();
        AdvanceSequence();
    }

    public void Exit()
    {
        m_uiMananger.Close();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (AdvanceSequence())
            {
                m_onFinishedCallback();
            }
        }
    }

    /// <summary>
    /// Call this whenever the user wants to advance the text
    /// </summary>
    /// <returns>Returns true if the text script has ended</returns>
    bool AdvanceSequence()
    {
        if (m_uiMananger.AdvanceSequence())    // Try to complete the current sequence first
        {
            int nextSequenceIndex = m_currentScriptSequenceIndex + 1;
            if (nextSequenceIndex < m_script.sequences.Count)
            {
                // Apply the next sequence
                var nextSequence = m_script.sequences[nextSequenceIndex];
                m_uiMananger.SetCurrentSequence(nextSequence);

                ++m_currentScriptSequenceIndex;
            }
            else
            {
                // We're at the end of the script, exit out
                return true;
            }
        }

        return false;
    }
}
