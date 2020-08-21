using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonscraperEngine;

public class DialogueTest : MonoBehaviour
{
    [SerializeField]
    string dialogueTestFile = "example_dialogue_1";
    [SerializeField]
    Localiser.Language languageToTest = Localiser.Language.Eng;
    [SerializeField]
    StandardDialogManager dialogueManager;
    [SerializeField]
    BackgroundBlending backgroundManager;
    [SerializeField]
    Texture2D finalTexture;

    DialogueScript m_testScript;
    StateMachine m_cutsceneStateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        Localiser.Instance.currentLanguage = languageToTest;

        TextAsset dialogueScript = Localiser.Instance.GetLocalisedDialogueFile(dialogueTestFile);
        m_testScript = JsonUtility.FromJson<DialogueScript>(dialogueScript.text);

        m_cutsceneStateMachine.currentState = new DialogueRulestate(m_cutsceneStateMachine, m_testScript, dialogueManager, 
            () => {
                dialogueManager.Close();
                m_cutsceneStateMachine.currentState = new BackgroundTransitionState(backgroundManager, finalTexture, () => { m_cutsceneStateMachine.currentState = null; });
            });
    }

    // Update is called once per frame
    void Update()
    {
        m_cutsceneStateMachine.Update();
    }
}
