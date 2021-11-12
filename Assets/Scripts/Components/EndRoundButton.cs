using UnityEngine;

public class EndRoundButton : MonoBehaviour
{
    public void EndRound()
    {
        GameController.i.EndCountdown();
    }
}
