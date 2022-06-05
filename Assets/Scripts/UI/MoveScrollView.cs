using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveScrollView : MonoBehaviour
{
    public ScrollRect Target;
    public float Step = 0.1f;
    
    // public float Lerp = 0.1f;
 
    public void Increment()
    {
        Target.horizontalNormalizedPosition = Mathf.Clamp(Target.horizontalNormalizedPosition + Step, 0, 1);
        // Target.horizontalNormalizedPosition = Mathf.Lerp(Target.horizontalNormalizedPosition, Target.horizontalNormalizedPosition + Step, Lerp);
    }
 
    public void Decrement()
    {
        Target.horizontalNormalizedPosition = Mathf.Clamp(Target.horizontalNormalizedPosition - Step, 0, 1);
        // Target.horizontalNormalizedPosition = Mathf.Lerp(Target.horizontalNormalizedPosition, Target.horizontalNormalizedPosition - Step, Lerp);
    }
}
