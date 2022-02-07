using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Setup")]
    public TextMeshProUGUI textField;
    public Button nextIndex;
    private float timeBetweenLetters = 0.0025f;

    [Header("History")]
    public HistoryData data;
    public InputField nameInput;
    public Text nameBox;
    public Image characterImage;
    public Sprite menSprite;
    public Sprite womanSprite;

    [Header("Dialogue Setup")]
    public bool useTranslateSystem = false;
    public DialogueEvent[] dialogue;

    [Header("Events")]
    public UnityEvent endDialogueEvent = new UnityEvent();
    public UnityEvent startDialogueEvent = new UnityEvent();

    private int index = 0;
    private bool canNext = true;
    private float timeBetweenIndexs = 0.1f;
    private bool runnigText = false;

    void Start()
    {
        startDialogueEvent.Invoke();
        nextIndex.onClick.AddListener(() => Next());
        SetDialogueText();
        SetCharacterImage();
    }

    public void LoadSceneTest(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void SetName()
    {
        data.SetPlayerName(nameInput.text);
    }

    public void SetBoxWithPlayerName()
    {
        nameBox.text = data.GetPlayerName();
    }

    public void SetCharacter(string characterName)
    {
        data.SetCharacter(characterName);
        SetCharacterImage();
    }

    public void SetCharacterImage()
    {
        if (data.GetCharacter() == CharacterSelected.Men)
        {
            characterImage.sprite = menSprite;
        }
        else characterImage.sprite = womanSprite;
    }

    public void Next()
    {
        if (runnigText)
        {
            StopAllCoroutines();
            var sentence = GetText();
            sentence = CheckString(sentence);
            textField.text = sentence;
            runnigText = false;

            if (dialogue[index].unityEvent != null)
            {
                dialogue[index].unityEvent.Invoke();
            }

            return;
        }

        if (index < dialogue.Length - 1)
        {
            if (canNext) StartCoroutine(NextText());
        }
        else endDialogueEvent.Invoke();
    }

    private IEnumerator NextText()
    {
        canNext = false;

        yield return new WaitForSeconds(timeBetweenIndexs);

        index++;

        SetDialogueText();

        canNext = true;
    }

    private void SetDialogueText()
    {
        string sentence = "";
        sentence = GetText();

        if (sentence == "")
        {
            textField.text = sentence;

            if (dialogue[index].unityEvent != null)
            {
                dialogue[index].unityEvent.Invoke();
            }

            runnigText = false;

            if (index < dialogue.Length - 1)
            {
                if (canNext) StartCoroutine(NextText());
            }
            else endDialogueEvent.Invoke();

            return;
        }

        StartCoroutine(TextRoutine(sentence));
    }

    private string GetText()
    {
        if (useTranslateSystem)
        {
            if (dialogue[index].dialogue == "") return "";
            return LanguageManager.Instance.GetKeyValue(dialogue[index].dialogue);
        }
        else return dialogue[index].dialogue;
    }

    public IEnumerator TextRoutine(string sentence)
    {
        runnigText = true;
        textField.text = "";

        sentence = CheckString(sentence);

        foreach (var letter in sentence)
        {
            textField.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        runnigText = false;

        if (dialogue[index].unityEvent != null)
        {
            dialogue[index].unityEvent.Invoke();
        }
    }

    private string CheckString(string sentence)
    {
        if (sentence.Contains("PLAYER"))
        {
            Debug.Log("Constains");

            var a = sentence.IndexOf("PLAYER");
            sentence = sentence.Remove(a, 6);
            var final = sentence.Insert(a, data.GetPlayerName());

            return final;
        }
        else return sentence;
    }

    [System.Serializable]
    public struct DialogueEvent
    {
        public string dialogue;
        public UnityEvent unityEvent;
    }
}
