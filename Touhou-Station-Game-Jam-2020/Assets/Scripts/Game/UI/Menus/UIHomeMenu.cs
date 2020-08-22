using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHomeMenu : FrontendMenuBase
{
    public void PlayGame()
    {
        FrontendStateManager.Instance.InitGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
