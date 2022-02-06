using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [Header("Fade Setup")]
    public float decreaseTime = 0.01f;
    [Range(0, 0.1f)] public float alphaDecrease = 0.01f;
    public bool runOnStart = false;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        if (runOnStart) 
            StartCoroutine( ColorRoutine() );
    }
    
    private IEnumerator ColorRoutine()
    {
        while (image.color.a > 0)
        {
            var tempColor = image.color;
            tempColor.a -= 0.01f;

            yield return new WaitForSeconds(0.01f);

            image.color = tempColor;
        }

        Destroy(this.gameObject);
    }
}
