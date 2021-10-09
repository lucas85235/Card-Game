using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [Header("Life Settings")]
    public Slider lifeSlider;
    public Text lifeText;
    public float maxLife = 100;

    [Header("Death Setup")]
    public bool destroyAfterDeath = true;

    [Header("Death Event")]
    public UnityEvent OnDeath;

    private float currentLife;

    void Start()
    {
        if (lifeSlider == null) {
            Debug.LogError("lifeSlider is Null");
            return;
        }
        
        lifeSlider.maxValue = maxLife;
        lifeSlider.value = maxLife;
        currentLife = lifeSlider.value;

        UpdateLifeSlider();
    }

    public void AddLife(float increment)
    {
        currentLife += increment;

        LifeRules();
        UpdateLifeSlider();
    }

    public void TakeDamage(float decrement)
    {
        currentLife -= decrement;

        LifeRules();
        UpdateLifeSlider();
    }

    private void DeathHandle()
    {
        Debug.Log("Character is Death");
        if (destroyAfterDeath) Destroy(this.gameObject);
        OnDeath?.Invoke();
    }

    private void LifeRules()
    {
        if (currentLife > maxLife) 
            currentLife = maxLife;
        if (currentLife < 1)
            DeathHandle();        
    }    

    private void UpdateLifeSlider()
    {
        if (lifeSlider != null)
            lifeSlider.value = currentLife;

        if (lifeText != null)
            lifeText.text = currentLife + " / " + maxLife;
    }
}
