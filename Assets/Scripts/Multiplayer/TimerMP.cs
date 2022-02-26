using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TimerMP
{
    private double timer = 20;
    private bool startTimer = false;
    private double timerIncrementValue;
    private double startTime;
    private Hashtable CustomeValue;

    public double Timer {
        get => timer;
        set => timer = value;
    }

    public double TimerIncrementValue {
        get => timerIncrementValue;
    }

    public bool StartTimer {
        get => startTimer;
    }

    public void SetTimerProperties()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CustomeValue = new Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
    }

    public void CountTimer(Action End = null)
    {
        if (!startTimer) return;

        timerIncrementValue = PhotonNetwork.Time - startTime;

        if (timerIncrementValue >= timer)
        {
            StopTimer();
            End?.Invoke();
        }
    }

    public IEnumerator Wait()
    {
        var lastStartTime = startTime;

        yield return new WaitUntil( () => lastStartTime != double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString()) );

        if (!PhotonNetwork.IsMasterClient)
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
    }

    public void StopTimer() => startTimer = false;
}
