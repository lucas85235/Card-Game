using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetInventory : MonoBehaviour
{
    private void OnEnable()
    {
        var parts = FindObjectsOfType<PartOptionButton>();

        for (int i = 0; i < DataManager.Instance.SaveCodes.Count; i++)
        {
            foreach (var item in parts)
            {
                if (item.PartCode == DataManager.Instance.SaveCodes[i])
                    item.GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
