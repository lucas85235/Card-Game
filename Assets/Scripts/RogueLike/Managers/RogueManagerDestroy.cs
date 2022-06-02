using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueManagerDestroy : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(RogueManager.Instance.gameObject);
    }
}
