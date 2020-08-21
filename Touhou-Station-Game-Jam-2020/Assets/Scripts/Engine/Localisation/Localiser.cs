using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonscraperEngine;
using System;

#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

class LocalisationPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    /// <summary>
    /// Make sure the dialogue translations are up to date based on what's in the folders. 
    /// </summary>
    /// <param name="report"></param>
    public void OnPreprocessBuild(BuildReport report)
    {
        GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/LocalisationManager.prefab", typeof(GameObject)) as GameObject;
        Debug.Assert(prefab);

        prefab.GetComponent<Localiser>().PopulateDialogTranslations();
    }
}
#endif

[UnitySingleton(UnitySingletonAttribute.Type.LoadedFromResources, false, "Prefabs/LocalisationManager")]
public class Localiser : UnitySingleton<Localiser>
{
    public enum Language
    {
        Eng,
        Jpn,
    }

    const string c_engId = "_eng";
    const string c_jpnId = "_jpn";

    Language m_currentLanguage = Language.Eng;
    Dictionary<string, EnumLookupTable<Language, string>> m_localisationDict = new Dictionary<string, EnumLookupTable<Language, string>>();

    [SerializeField]
    TextAsset m_strings;

    public Language currentLanguage
    {
        get { return m_currentLanguage; }
        set { m_currentLanguage = value; }
    }

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

    [Serializable]
    public struct Translation
    {
        public string key;
        public TextAsset[] translations;
    }

    [HideInInspector]
    [SerializeField]
    Translation[] m_dialogTranslations;  // This is automatically populated. See PopulateDialogTranslations().
    Dictionary<string, Translation> m_dialogTranslationLookup = new Dictionary<string, Translation>();

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

    new private void Awake()
    {
        base.Awake();

        //DummySaveStrings();

#if UNITY_EDITOR
        PopulateDialogTranslations();
#endif
        foreach (var translation in m_dialogTranslations)
        {
            m_dialogTranslationLookup.Add(translation.key, translation);
        }

        Debug.Log("Dialog translations count = " + m_dialogTranslationLookup.Count);

        LoadStrings();
    }

    /// <summary>
    /// Returns the translation string for the currently set language
    /// </summary>
    public string GetLocalised(string key)
    {
        EnumLookupTable<Language, string> translations;
        if (m_localisationDict.TryGetValue(key, out translations) && !string.IsNullOrEmpty(translations[m_currentLanguage]))
        {
            return translations[m_currentLanguage];
        }

        return "[miss_str_" + key + "]";
    }

    /// <summary>
    /// Get the approprite dialogue file for requested key
    /// </summary>
    /// <param name="key">The filename of the dialogue file without the file path, extention or language suffix.</param>
    /// <returns></returns>
    public TextAsset GetLocalisedDialogueFile(string key)
    {
        Translation translation;
        if (m_dialogTranslationLookup.TryGetValue(key, out translation))
        {
            TextAsset asset = translation.translations[(int)m_currentLanguage];
            if (!asset)
            {
                // try to use english by default
                Debug.LogWarning("Unable to find dialogue sequence with key " + key + " for language " + m_currentLanguage + ". Defaulting to english.");
                asset = translation.translations[(int)Language.Eng];
            }

            Debug.Assert(asset, "Unable to find dialogue sequence with key " + key);

            return asset;
        }

        Debug.LogError("Unable to find dialogue sequence with key " + key);

        return null;
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

#if UNITY_EDITOR
    public void PopulateDialogTranslations()
    {
        Debug.Log("Populating dialogue translations");

        Dictionary<string, Translation> translations = new Dictionary<string, Translation>();
        string[] filePaths = System.IO.Directory.GetFiles("Assets/TextAssets/DialogueScripts/", "*.json");

        foreach (string assetPath in filePaths)
        {
            TextAsset objAsset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset)) as TextAsset;
            Debug.Log("Found dialogue file " + objAsset.name);

            string key = objAsset.name;
            key = key.Replace(c_engId, "");
            key = key.Replace(c_jpnId, "");

            Translation translation;
            if (!translations.TryGetValue(key, out translation))
            {
                translation = new Translation();
                translation.key = key;
                translation.translations = new TextAsset[EnumX<Language>.Count];
                translations.Add(key, translation);
            }

            if (objAsset.name.EndsWith(c_jpnId))
            {
                translation.translations[(int)Language.Jpn] = objAsset;
            }
            else // if (objAsset.name.Contains(c_engId))
            {
                // Must be english by default
                translation.translations[(int)Language.Eng] = objAsset;
            }
        }

        List<Translation> finalTranslationList = new List<Translation>();
        foreach (var keyVal in translations)
        {
            finalTranslationList.Add(keyVal.Value);
        }

        m_dialogTranslations = finalTranslationList.ToArray();
    }
#endif
}
