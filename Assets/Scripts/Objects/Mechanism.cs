using UnityEngine;

public class Mechanism : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public virtual void On()
    {
        if (HelperFunctions.AnimatorIsPlaying(animator)) return;
        animator.SetTrigger("On");
        
    }

    public virtual void Off()
    {
        if (HelperFunctions.AnimatorIsPlaying(animator)) return;
        animator.SetTrigger("Off");
    }
}
