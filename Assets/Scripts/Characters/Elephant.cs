using UnityEngine;

public class Elephant : Character
{
    [HideInInspector] public bool elephantDoingPlatform;
    [HideInInspector] public bool elephantDoingCurve;

    public void DoPlatform()
    {
        animator.SetBool("ElephantDoingPlatform", true);
        ReleaseCurve();
    }
    public void ReleasePlatform()
    {
        animator.SetBool("ElephantDoingPlatform", false);
    }

    public void DoCurve()
    {
        animator.SetBool("ElephantDoingCurve", true);
        ReleasePlatform();
    }

    public void ReleaseCurve()
    {
        animator.SetBool("ElephantDoingCurve", false);
    }

    protected override void Awake()
    {
        base.Awake();

        this.jumpHeight = 1;
        this.movementSpeed = 1;
    }
}
