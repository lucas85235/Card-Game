using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRawRobot : MonoBehaviour
{
    public Robot robot;

    private void Start()
    {
        TryGetComponent(out RobotAnimation animation);

        if (animation != null)
            animation.ChangeRobotSprites(robot.Data);
    }
}
