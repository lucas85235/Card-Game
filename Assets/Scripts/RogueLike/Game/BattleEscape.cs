using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEscape : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        
        if (RogueItems.Instance.Smokes == 0)
        {
            button.interactable = false;
            return;
        }

        button.onClick.AddListener(Escape);
    }

    private void Escape()
    {
        RogueItems.Instance.Smokes -= 1;
        FindObjectOfType<SceneLoader>().LoadScene("MenuRogueLike");
    }
}
