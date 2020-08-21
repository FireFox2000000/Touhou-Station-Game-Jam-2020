using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILanguageSelectionMenu : FrontendMenuBase
{
    [SerializeField]
    FrontendMenuBase m_nextState;

    void SelectLanguage(Localiser.Language langauge)
    {
        Debug.Log("User selected language " + langauge);

        Localiser.Instance.currentLanguage = langauge;
        Localiser.Instance.LocaliseSceneTextElements();

        // Advance to the frontend menu
        ChangeState(m_nextState);
    }

    public void SelectEnglish()
    {
        SelectLanguage(Localiser.Language.Eng);
    }

    public void SelectJapanese()
    {
        SelectLanguage(Localiser.Language.Jpn);
    }
}
