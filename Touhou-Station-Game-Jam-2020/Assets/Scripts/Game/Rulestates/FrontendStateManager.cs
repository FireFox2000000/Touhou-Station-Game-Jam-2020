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
    BackgroundBlending m_backgroundManager;
    [SerializeField]
    Texture2D m_initialTexture;
    [SerializeField]
    string introCutsceneKey;
    [SerializeField]
    GameObject[] m_dummyObjectsToDisable;

    StateMachine m_stateMachine = new StateMachine();

    void Start()
    {
        m_backgroundManager.SetTexture(m_initialTexture);

        foreach(var go in m_dummyObjectsToDisable)
        {
            go.SetActive(false);
        }

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
        var fadeBlocker = GetUIElement<UIFadeBlocker>();
        fadeBlocker.FadeIn(OnPlayGameTransitionFadeComplete);
        var music = GetComponent<AudioSource>();
        StartCoroutine(FadeOut(music, fadeBlocker.fadeSpeed * 0.5f));
    }

    void OnPlayGameTransitionFadeComplete()
    {
        SceneManager.LoadSceneAsync(1);
    }

    static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
