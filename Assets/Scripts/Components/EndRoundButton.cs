using UnityEngine;

public class EndRoundButton : MonoBehaviour
{
    public GameObject disablePopup;

    private void Start()
    {
        Round.i.EndTurn.AddListener(Disable);    
    }

    private void Disable()
    {
        disablePopup.SetActive(false);
    }

    public void EndRound()
    {
        GameController.i.EndCountdown();
    }
}
