using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLife : MonoBehaviour
{
    [Header("Setup")]
    public Life playerLife;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => RogueLife.Instance.isReady);

        playerLife.SetLife(PlayerPrefs.GetInt("SAVE-LIFE"));
        playerLife.OnLifeUpdate.AddListener(() =>
        {
            RogueLife.Instance.Life = playerLife.CurrentLife;
        });
    }
}
