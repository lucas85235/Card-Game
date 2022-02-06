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
        
        nextIndex.onClick.AddListener(() =>
        {
            if (runnigText)
            {
                StopAllCoroutines();
                textField.text = GetText();
                runnigText = false;
                return;
            } 

            if (index < dialogue.Length - 1)
            {
                if (canNext) StartCoroutine(NextText());
            }
            else endDialogueEvent.Invoke();
        });

        SetDialogueText();
    }

    public void LoadSceneTest(string scene)
    {
        SceneManager.LoadScene(scene);
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

        StartCoroutine( TextRoutine(setence) );

        if (dialogue[index].unityEvent != null)
        {
            dialogue[index].unityEvent.Invoke();
        }
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
    }

    [System.Serializable]
    public struct DialogueEvent
    {
        public string dialogue;
        public UnityEvent unityEvent;
    }
}
