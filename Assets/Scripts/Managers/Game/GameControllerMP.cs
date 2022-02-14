using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerMP : GameController
{

    protected override IEnumerator Start()
    {
        if (isMultiplayer)
        {
            yield return new WaitUntil( () => BasicConection.Instance.IsReady());
        }
        
        base.Start();
    }

}
