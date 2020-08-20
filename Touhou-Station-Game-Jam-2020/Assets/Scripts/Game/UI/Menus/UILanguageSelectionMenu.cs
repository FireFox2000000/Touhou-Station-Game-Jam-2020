using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILanguageSelectionMenu : MonoBehaviour
{
    void SelectLanguage(Localiser.Language langauge)
    {
        Localiser.Instance.currentLanguage = langauge;

        Debug.Log("User selected language " + langauge);

        // Todo, advance to the frontend menu
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
