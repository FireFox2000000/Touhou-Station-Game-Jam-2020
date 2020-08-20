using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;
using System;

[UnitySingleton(UnitySingletonAttribute.Type.LoadedFromResources, false, "Prefabs/LocalisationManager")]
public class Localiser : UnitySingleton<Localiser>
{
    public enum Language
    {
        Eng,
        Jpn,
    }

    Language m_currentLanguage = Language.Eng;
    Dictionary<string, EnumLookupTable<Language, string>> m_localisationDict = new Dictionary<string, EnumLookupTable<Language, string>>();

    [SerializeField]
    TextAsset m_strings;

    [Serializable]
    class TranslationSaveData
    {
        [Serializable]
        public class Translation
        {
            public string key;
            public string eng;
            public string jpn;

            public void PopulateToTable(EnumLookupTable<Language, string> table)
            {
                table[Language.Eng] = eng;
                table[Language.Jpn] = jpn;
            }

            public void PopulateFromTable(EnumLookupTable<Language, string> table)
            {
                eng = table[Language.Eng];
                jpn = table[Language.Jpn];
            }
        }

        public List<Translation> translationStrings = new List<Translation>();
    }

    void LoadStrings()
    {
        if (!m_strings) return;

        var strings = JsonUtility.FromJson<TranslationSaveData>(m_strings.text);

        foreach (var str in strings.translationStrings)
        {
            var localisedStrings = new EnumLookupTable<Language, string>();
            str.PopulateToTable(localisedStrings);

            m_localisationDict.Add(str.key, localisedStrings);
        }
    } 

    void DummySaveStrings()
    {
        var localisationDict = new Dictionary<string, EnumLookupTable<Language, string>>();

        {
            EnumLookupTable<Language, string> localisedStrings = new EnumLookupTable<Language, string>();
            localisedStrings[Language.Eng] = "Play Game";
            localisedStrings[Language.Jpn] = "Play Game (Jpn)";

            localisationDict.Add("MenuItem1", localisedStrings);
        }

        {
            EnumLookupTable<Language, string> localisedStrings = new EnumLookupTable<Language, string>();
            localisedStrings[Language.Eng] = "Exit Game";
            localisedStrings[Language.Jpn] = "Exit Game (Jpn)";

            localisationDict.Add("MenuItem2", localisedStrings);
        }

        TranslationSaveData saveData = new TranslationSaveData();

        foreach (var locale in localisationDict)
        {
            var saveLocale = new TranslationSaveData.Translation();
            saveLocale.key = locale.Key;
            saveLocale.PopulateFromTable(locale.Value);

            saveData.translationStrings.Add(saveLocale);
        }

        string json = JsonUtility.ToJson(saveData, true);
        Debug.Log(json);
        System.IO.File.WriteAllText("strings.json", json);
    }

    private void Awake()
    {
        //DummySaveStrings();

        LoadStrings();
    }

    /// <summary>
    /// Returns the translation string for the currently set language
    /// </summary>
    public string GetLocalised(string key)
    {
        EnumLookupTable<Language, string> translations;
        if (m_localisationDict.TryGetValue(key, out translations))
        {
            return translations[m_currentLanguage];
        }

        return "[miss_str_" + key + "]";
    }

    /// <summary>
    /// Walks through all TMPro.TextMeshPro, TMPro.TextMeshProUGUI and UnityEngine.UI.Text components and replaces it's text with that loaded from the localisation strings.
    /// Has no effect if the original string is empty.
    /// </summary>
    public void LocaliseSceneTextElements()
    {
        GameObject[] gos = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject go in gos)
        {
            if (go && go.transform.parent == null)
            {
                // UnityEngine.UI.Text
                {
                    var textElements = go.GetComponentsInChildren<UnityEngine.UI.Text>(true);
                    foreach (var textElement in textElements)
                    {
                        if (!string.IsNullOrEmpty(textElement.text))
                            textElement.text = GetLocalised(textElement.text);
                    }
                }

                // TMPro.TextMeshPro
                {
                    var textElements = go.GetComponentsInChildren<TMPro.TextMeshPro>(true);
                    foreach (var textElement in textElements)
                    {
                        if (!string.IsNullOrEmpty(textElement.text))
                            textElement.text = GetLocalised(textElement.text);
                    }
                }

                // TMPro.TextMeshProUGUI
                {
                    var textElements = go.GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
                    foreach (var textElement in textElements)
                    {
                        if (!string.IsNullOrEmpty(textElement.text))
                            textElement.text = GetLocalised(textElement.text);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Similar to "Start()" and "Update()" on Monobehaviour components, this will invoke all "OnLocalise()" methods found on components.
    /// Useful for manually controlling localisation.
    /// </summary>
    public static void LocaliseSceneBroadcast()
    {
        GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in gos)
        {
            if (go && go.transform.parent == null)
            {
                go.gameObject.BroadcastMessage("OnLocalise", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
