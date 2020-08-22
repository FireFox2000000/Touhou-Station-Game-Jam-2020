using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;
using UnityEngine.SceneManagement;

[UnitySingleton(UnitySingletonAttribute.Type.ExistsInScene, true)]
public class FrontendStateManager : UnitySingleton<FrontendStateManager>
{
    [SerializeField]
    FrontendMenuBase m_initialState;
    [SerializeField]
    StandardDialogManager m_dialogueManager;
    [SerializeField]
    string introCutsceneKey;

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

    public void InitGame()
    {
        TextAsset introCutscene = Localiser.Instance.GetLocalisedDialogueFile(introCutsceneKey);
        DialogueScript script = JsonUtility.FromJson<DialogueScript>(introCutscene.text);
        m_stateMachine.currentState = new DialogueRulestate(m_stateMachine, script, m_dialogueManager, TransitionToGameplay);
    }

    void TransitionToGameplay()
    {
        m_stateMachine.currentState = null;
        GetUIElement<UIFadeBlocker>().FadeIn(OnPlayGameTransitionFadeComplete);
    }

    void OnPlayGameTransitionFadeComplete()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
