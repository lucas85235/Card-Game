using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LanguageManager : MonoBehaviour 
{    
    // colocar os arquivos de liguagem na pasta Resources
    // eles devem ser um json porem devem ter a extensão txt
    // devido a um bug que no android não e reconhecido caso
    // o arquivo tenha a extensão json

    [HideInInspector]
    public const string saveLanguageKey = "Language";
    public string SaveLanguageKey { get => saveLanguageKey; }
    private int languageSelected = 1;

    [HideInInspector] public string[] languageOptions = {
        "language_text_pt-br",
        "language_text_en-us",
    };

    private Dictionary<string, string> localizedText;
    private string missingText = "Text or key not found!";
    private bool isReady = false;

    [Header("Use para mudar a linguagem em tempo de execução")]
    public Languages selectLanguage;

    public string GetSaveLanguage() => PlayerPrefs.GetString(SaveLanguageKey);

    /// <summary> use para saber se já poder usar o sistema </summary>
    public bool IsReady() => isReady;

    /// <summary> salva a linguagem que for igual ao nome do arquivo </summary>
    public void SaveLanguage(int fileIndex) 
    {
        PlayerPrefs.SetInt(saveLanguageKey, fileIndex);
        PlayerPrefs.Save();
    }

    /// <summary> carrega a linguagem que foi salve ou a padrão caso não tenha salvo </summary>
    public void LoadSaveLanguage() => LoadLocalizedText(languageSelected);

    /// <summary> Use esse evento para trocar os textos após a mudança de linguagem </summary>
    public Action OnChangeLanguage;

    public static LanguageManager Instance;

    private int currentLanguageIndex;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (selectLanguage == Languages.ptBr)
        {
            languageSelected = 0;
        }
        if (selectLanguage == Languages.enUs)
        {
            languageSelected = 1;
        }
        
        if (!PlayerPrefs.HasKey(saveLanguageKey))
        {
            PlayerPrefs.SetInt(saveLanguageKey, languageSelected);
            PlayerPrefs.Save();
        }

        // Load Saved Key
        languageSelected = PlayerPrefs.GetInt(saveLanguageKey);
        LoadLocalizedText(languageSelected);
    }

    // acha e carrega o arquivo com as traduções
    public void LoadLocalizedText(int navigationValue=0) 
    {
        isReady = false;

        int fileToGet = languageSelected + navigationValue;

        if(fileToGet >= languageOptions.Length)
        {
            fileToGet = 0;
        }
        else if(fileToGet < 0)
        {
            fileToGet = languageOptions.Length - 1;
        }

        localizedText = new Dictionary<string, string>();

        TextAsset file = Resources.Load("Localization/" + languageOptions[fileToGet]) as TextAsset;
        var dataAsJson = file.text;
        LanguageData loaderData = JsonUtility.FromJson<LanguageData>(dataAsJson);
        
        for (int i = 0; i < loaderData.items.Length; i++)
        {
            localizedText.Add(loaderData.items[i].key, loaderData.items[i].value);
        }

        SaveLanguage(fileToGet);

        PlayerPrefs.SetInt(saveLanguageKey, fileToGet);
        PlayerPrefs.Save();

        languageSelected = fileToGet;

        isReady = true;

        if (OnChangeLanguage != null) OnChangeLanguage();
        
        Debug.Log("Data loader, dictionary contains: " + localizedText.Count + " entries");
    }

    // retorna o valor definido para a chave
    public string GetKeyValue(string key) 
    {
        if (!isReady)
        {
            LoadLocalizedText(languageSelected);
        }
        string resul = missingText;

        if (localizedText.ContainsKey(key)) 
        {
            resul = localizedText[key];
        }

        return resul;
    }
}

public enum Languages
{
    ptBr,
    enUs,
}
