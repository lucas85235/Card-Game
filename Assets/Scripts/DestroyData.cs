using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyData : MonoBehaviour
{
    public void DestroyDataObject()
    {
        if (DataManager.Instance.gameObject != null)
            Destroy(DataManager.Instance.gameObject);
    }
}
