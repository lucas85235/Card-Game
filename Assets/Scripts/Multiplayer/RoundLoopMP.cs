using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoundLoopMP : Round
{
    [Header("Setup")]
    [SerializeField] private bool useTimer = true;
    [SerializeField] private float timeToPlay = 10f;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Text timeText;

    private TimerMP timer = new TimerMP();

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length > 0);

        timer.Timer = timeToPlay;
        timeSlider.gameObject.SetActive(useTimer);
        timeSlider.maxValue = timeToPlay;
        
        if (useTimer)
        {
            Round.i.EndTurn.AddListener(() => timer.SetTimerProperties());
            timer.SetTimerProperties();
        }
    }

    private void Update()
    {
        timer.CountTimer(() =>
        {
            timeSlider.gameObject.SetActive(false);
            Round.i.StartTurn?.Invoke();
        });

        if (timer.StartTimer)
        {
            float totalTime = timeToPlay;
            timeSlider.value = totalTime - (float) timer.TimerIncrementValue;
            timeText.text = timer.TimerIncrementValue.ToString("#");
        }
    }
}
