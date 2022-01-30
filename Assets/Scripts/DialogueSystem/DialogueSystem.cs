using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Setup")]
    public Text textField;
    public Button nextIndex;

    [Header("Dialogue Setup")]
    public bool useTranslateSystem = false;
    public DialogueEvent[] dialogue;

    [Header("Events")]
    public UnityEvent endDialogueEvent = new UnityEvent();
    public UnityEvent startDialogueEvent = new UnityEvent();

    protected int index = 0;
    protected bool canNext = true;
    private float timeBetweenIndexs = 0.2f;

    void Start()
    {
        startDialogueEvent.Invoke();
        
        nextIndex.onClick.AddListener(() =>
        {
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
        if (useTranslateSystem)
        {
            textField.text = LanguageManager.Instance.GetKeyValue(dialogue[index].dialogue);
        }
        else textField.text = dialogue[index].dialogue;

        if (dialogue[index].unityEvent != null)
        {
            dialogue[index].unityEvent.Invoke();
        }
    }

    [System.Serializable]
    public struct DialogueEvent
    {
        public string dialogue;
        public UnityEvent unityEvent;
    }
}
