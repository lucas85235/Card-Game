using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocazidedTextEvent : MonoBehaviour
{
    public string sceneToLoad = "Loading";
    public Languages language;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            StartCoroutine(ChangeLanguage());
        });
    }

    private IEnumerator ChangeLanguage()
    {
        if (LanguageManager.Instance.selectLanguage == language) yield break;

        LanguageManager.Instance.selectLanguage = language;

        yield return null;

        if (language == Languages.ptBr)
            PlayerPrefs.SetString(LanguageManager.Instance.SaveLanguageKey, LanguageManager.Instance.languageOptions[0]);
        else if (language == Languages.enUs)
            PlayerPrefs.SetString(LanguageManager.Instance.SaveLanguageKey, LanguageManager.Instance.languageOptions[1]);

        PlayerPrefs.Save();

        SceneManager.LoadScene(sceneToLoad);
    }
}
