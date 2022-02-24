using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerSolo : GameController
{
    protected override void Start() 
    {
        base.Start();
        
        if (useTimer)
        {
            Round.i.EndTurn.AddListener(() => StartCountdown());            
            StartCountdown();
        }
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
