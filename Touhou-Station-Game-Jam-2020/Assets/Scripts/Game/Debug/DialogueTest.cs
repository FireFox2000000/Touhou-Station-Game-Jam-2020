using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonscraperEngine;

public class DialogueTest : MonoBehaviour, IDialogueUI
{
    [SerializeField]
    Text text;
    [SerializeField]
    TextAsset dialogueScript;

    public bool AdvanceSequence()
    {
        return true;
    }

    public void SetCurrentSequence(DialogueScript.Sequence sequence)
    {
        text.text = sequence.text;
    }

    DialogueScript m_testScript;
    StateMachine m_cutsceneStateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        m_testScript = JsonUtility.FromJson<DialogueScript>(dialogueScript.text);

        m_cutsceneStateMachine.currentState = new DialogueRulestate(m_cutsceneStateMachine, m_testScript, this, () => { Close(); m_cutsceneStateMachine.currentState = null; });

        //string jsonScript = JsonUtility.ToJson(m_testScript, true);
        //System.IO.File.WriteAllText("test_dialogue_1.json", jsonScript);
    }

    // Update is called once per frame
    void Update()
    {
        m_cutsceneStateMachine.Update();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
