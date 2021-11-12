using UnityEngine;
using UnityEngine.Events;

public class Round : MonoBehaviour
{    
    [Header("EVENTS")]
    public UnityEvent StartTurn;
    public UnityEvent EndTurn;

    // Make dependency injection for the events below
    
    public UnityEvent<CardImage> UseCard;
    public UnityEvent<Robot, Robot> RobotAttack;

    //

    public static Round i;

    protected virtual void Awake()
    {
        i = this;
    }

    protected virtual void OnDestroy()
    {
        StartTurn.RemoveAllListeners();
        EndTurn.RemoveAllListeners();

        StartTurn = null;
        EndTurn = null;
    }
}
