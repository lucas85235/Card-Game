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
    public float timeBetweenLetters = 0.05f;

    [Header("Dialogue Setup")]
    public bool useTranslateSystem = false;
    public DialogueEvent[] dialogue;

    [Header("Events")]
    public UnityEvent endDialogueEvent = new UnityEvent();
    public UnityEvent startDialogueEvent = new UnityEvent();

    private int index = 0;
    private bool canNext = true;
    private float timeBetweenIndexs = 0.2f;
    private bool runnigText = false;

    void Start()
    {
        startDialogueEvent.Invoke();
        nextIndex.onClick.AddListener(() => Next());
        SetDialogueText();
        CheckString();
    }

    public void LoadSceneTest(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Next()
    {
        if (runnigText)
        {
            StopAllCoroutines();
            textField.text = GetText();
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
        string setence = "";
        setence = GetText();

        StartCoroutine(TextRoutine(setence));
    }

    private string GetText()
    {
        if (useTranslateSystem)
        {
            return LanguageManager.Instance.GetKeyValue(dialogue[index].dialogue);
        }
        else return dialogue[index].dialogue;
    }

    public IEnumerator TextRoutine(string setence)
    {
        runnigText = true;
        textField.text = "";

        foreach (var letter in setence)
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

    private void CheckString()
    {
        var tempString = "12 P a PLAYER 11";

        if (tempString.Contains("PLAYER"))
        {
            Debug.Log("Constains");

            var a = tempString.IndexOf("PLAYER");
            tempString = tempString.Remove(a, 6);
            var f = tempString.Insert(a, "jogador");

            Debug.Log(a);
            Debug.Log(tempString);
            Debug.Log(f);
        }
    }

    [System.Serializable]
    public struct DialogueEvent
    {
        public string dialogue;
        public UnityEvent unityEvent;
    }
}
