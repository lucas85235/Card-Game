using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Robot))]

public class Life : MonoBehaviour
{
    [Header("Life Settings")]
    public Slider lifeSlider;
    public Text lifeText;

    [Header("Death Setup")]
    public bool destroyAfterDeath = true;
    public bool isDeath = false;

    [Header("Death Event")]
    public UnityEvent OnDeath;

    private Robot robot;
    private int maxLife;
    private int currentLife;
    
    void Start()
    {
        robot = GetComponent<Robot>();

        if (lifeSlider == null) {
            Debug.LogError("lifeSlider is Null");
            return;
        }
        
        maxLife = robot.data.health;
        lifeSlider.maxValue = maxLife;
        lifeSlider.value = maxLife;
        currentLife = (int) lifeSlider.value;

        UpdateLifeSlider();
    }

    public void AddLife(int increment)
    {
        currentLife += increment;

        LifeRules();
        UpdateLifeSlider();
    }

    public void TakeDamage(int decrement)
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
        isDeath = true;
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
