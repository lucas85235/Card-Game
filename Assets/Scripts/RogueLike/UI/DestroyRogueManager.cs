using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRogueManager : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(RogueManager.Instance.gameObject);
    }
}
