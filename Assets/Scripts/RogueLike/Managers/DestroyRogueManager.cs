using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyRogueManager : MonoBehaviour
{
    public bool loadOtherScene = false;
    public string otherSceneName = "MenuRogueLike";

    public void Destroy()
    {
        RogueManager.Instance.OnApplicationQuit();
        Destroy(RogueManager.Instance.gameObject);

        if (loadOtherScene)
            SceneManager.LoadScene(otherSceneName);
    }
}
