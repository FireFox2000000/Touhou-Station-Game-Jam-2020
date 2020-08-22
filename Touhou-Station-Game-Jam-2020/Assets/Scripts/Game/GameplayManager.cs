using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[UnitySingleton(UnitySingletonAttribute.Type.ExistsInScene, true)]
public class GameplayManager : UnitySingleton<GameplayManager>
{
    public GameplayUIServices uiServices;
    bool m_loadingNextScene = false;

    MoonscraperEngine.Event m_userWonGameEvent = new MoonscraperEngine.Event();
    public MoonscraperEngine.Event UserWonGameEvent { get { return m_userWonGameEvent; } }

    // Start is called before the first frame update
    void Start()
    {
        UserWonGameEvent.Register(OnUserWonGame);
    }

    void OnUserWonGame()
    {
        if (m_loadingNextScene)
            return;

        m_loadingNextScene = true;

        var fadeBlocker = uiServices.GetComponentInChildren<UIFadeBlocker>(true);

        Camera mainCamera = Camera.main;
        AudioSource music = mainCamera.GetComponent<AudioSource>();

        if (music)
        {
            StartCoroutine(FadeOut(music, fadeBlocker.fadeSpeed * 0.5f));
        }

        // Open up the loading screen and transition to final scene once completed
        fadeBlocker.FadeIn(() => {
            SceneManager.LoadSceneAsync(2);
        });
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
