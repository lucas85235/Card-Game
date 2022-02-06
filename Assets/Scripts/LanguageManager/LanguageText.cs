using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public string key;

    private void Start()
    {
        TryGetComponent(out text);
        LanguageManager.Instance.OnChangeLanguage += UpdateText;

        UpdateText();
    }
    private void OnDestroy()
    {
        LanguageManager.Instance.OnChangeLanguage -= UpdateText;
    }

    private void UpdateText() 
    {
        text.text = LanguageManager.Instance.GetKeyValue(key);
    }
}
