using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerMP : GameController
{
    protected override IEnumerator Start()
    {
        yield return new WaitUntil( () => Round.i.isReady);
        
        robots.Add(Round.i.playerOne);
        robots.Add(Round.i.playerTwo);

        AudioManager.Instance.Play(AudiosList.gameplayMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 3f, startVolume: 0);
    }

    public override Robot GetTheOtherRobot(Robot emitterRobot)
    {
        foreach (var robot in robots)
        {
            if(robot != emitterRobot)
            {
                return robot;
            }
        }

        return null;
    }
}
