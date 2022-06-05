using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyRogueManager : MonoBehaviour
{
    [Header("Setup")]
    public bool loadOtherScene = false;
    private string otherSceneName = "Menu";

    public void Destroy()
    {
        RogueManager.Instance.OnApplicationQuit();
        Destroy(RogueManager.Instance.gameObject);

        if (loadOtherScene)
            TransitionManager.Instance.StartTransition(otherSceneName);
    }
}
