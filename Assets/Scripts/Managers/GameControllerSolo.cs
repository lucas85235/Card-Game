using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerSolo : GameController
{
    [Header("Setup")]
    [SerializeField] protected bool useTimer = true;
    [SerializeField] protected float timeToPlay;
    [SerializeField] protected Slider timeSlider;

    protected Coroutine timeRoundCoroutine;

    protected override IEnumerator Start() 
    {
        base.Start();
        
        yield return null;

        if (useTimer)
        {
            Round.i.EndTurn.AddListener(() => StartCountdown());            
            StartCountdown();
        }

        timeSlider.gameObject.SetActive(useTimer);
    }

    public override void EndCountdown()
    {
        if (useTimer)
        {
            if (timeRoundCoroutine == null) return;

            timeSlider.gameObject.SetActive(false);
            StopCoroutine(timeRoundCoroutine);            
        }

        Round.i.StartTurn?.Invoke();
    }

    protected void StartCountdown()
    {
        if (!useTimer)
        {
            return;
        }

        timeSlider.gameObject.SetActive(true);
        timeRoundCoroutine = StartCoroutine(Countdown());
    }

    protected IEnumerator Countdown()
    {
        float timeRemaining = timeToPlay;

        while (timeRemaining > 0)
        {
            yield return null;
            timeRemaining -= Time.deltaTime;
            timeSlider.value = timeRemaining / timeToPlay;
        }

        timeSlider.gameObject.SetActive(false);
        Round.i.StartTurn?.Invoke();
    }
}
