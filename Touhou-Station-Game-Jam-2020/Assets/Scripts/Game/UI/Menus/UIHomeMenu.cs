using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHomeMenu : FrontendMenuBase
{
    public void PlayGame()
    {
        FrontendStateManager.Instance.GetUIElement<UIFadeBlocker>().FadeIn(OnPlayGameTransitionFadeComplete);
    }

    void OnPlayGameTransitionFadeComplete()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
