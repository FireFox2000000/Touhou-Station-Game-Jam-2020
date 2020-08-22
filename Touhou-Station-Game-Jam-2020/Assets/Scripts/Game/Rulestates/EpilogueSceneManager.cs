using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;

public class EpilogueSceneManager : MonoBehaviour
{
    [SerializeField]
    BackgroundBlending m_backgroundManager;
    [SerializeField]
    StandardDialogManager m_dialogueManager;
    [SerializeField]
    UIFadeBlocker m_loadingTransition;
    [SerializeField]
    string m_cutsceneKey;
    [SerializeField]
    Texture2D m_cutsceneStartTexture;
    [SerializeField]
    float m_dialogueStartDelay = 1.0f;
    [SerializeField]
    UIEndcardOptions m_endCardMenu;

    StateMachine m_stateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        m_endCardMenu.gameObject.SetActive(false);

        Localiser.Instance.LocaliseSceneTextElements();

        m_backgroundManager.SetTexture(m_cutsceneStartTexture);
        m_loadingTransition.FadeOut(() => {
            m_stateMachine.currentState = new DelayState(m_dialogueStartDelay, StartCutscene);
        });
    }

    void Update()
    {
        m_stateMachine.Update();
    }

    void StartCutscene()
    {
        TextAsset introCutscene = Localiser.Instance.GetLocalisedDialogueFile(m_cutsceneKey);
        DialogueScript script = JsonUtility.FromJson<DialogueScript>(introCutscene.text);
        m_stateMachine.currentState = new DialogueRulestate(m_stateMachine, script, m_dialogueManager, OnCutsceneEnd);
    }

    void OnCutsceneEnd()
    {
        // Display thanks for playing image or something
        m_stateMachine.currentState = new DelayState(m_dialogueStartDelay, () => {
            m_endCardMenu.gameObject.SetActive(true);
        });
    }
}
