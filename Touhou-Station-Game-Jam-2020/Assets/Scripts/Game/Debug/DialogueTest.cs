using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonscraperEngine;

public class DialogueTest : MonoBehaviour
{
    [SerializeField]
    TextAsset dialogueScript;
    [SerializeField]
    StandardDialogManager dialogueManager;

    DialogueScript m_testScript;
    StateMachine m_cutsceneStateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        m_testScript = JsonUtility.FromJson<DialogueScript>(dialogueScript.text);

        m_cutsceneStateMachine.currentState = new DialogueRulestate(m_cutsceneStateMachine, m_testScript, dialogueManager, 
            () => {
                dialogueManager.Close();
                m_cutsceneStateMachine.currentState = null;
            });
    }

    // Update is called once per frame
    void Update()
    {
        m_cutsceneStateMachine.Update();
    }
}
