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

    public string GetLocalised(string key)
    {
        EnumLookupTable<Language, string> translations;
        if (m_localisationDict.TryGetValue(key, out translations))
        {
            return translations[m_currentLanguage];
        }

        return "[miss_str_" + key + "]";
    }
}
