using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyData : MonoBehaviour
{
    public void DestroyDataObject()
    {
        Destroy(DataManager.Instance.gameObject);
    }
}
