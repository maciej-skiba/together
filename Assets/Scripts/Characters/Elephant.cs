using System.Runtime.CompilerServices;
using UnityEngine;

public class Elephant : Character
{
    [HideInInspector] public bool elephantDoingPlatform;
    [HideInInspector] public bool elephantDoingCurve;
    [HideInInspector] public bool isHoldingPlatform = false;
    [HideInInspector] public bool isHoldingCurve = false;

    [SerializeField] private PolygonCollider2D idleColliderObj;
    [SerializeField] private PolygonCollider2D eleHoldingPlatformColliderObj;
    [SerializeField] private PolygonCollider2D curveColliderObj;
    [SerializeField] private BoxCollider2D platformColliderObj;
    [SerializeField] private AnimationClip platformAnimClip;
    [SerializeField] private AnimationClip curveAnimClip;
    [SerializeField] private AnimationClip platformToCurveAnimClip;
    [SerializeField] private AnimationClip platformToIdleAnimClip;
    [SerializeField] private AnimationClip curveToPlatformAnimClip;
    [SerializeField] private AnimationClip curveToIdleAnimClip;
    [SerializeField] private AnimationClip launchPlatformAnimClip;
    [SerializeField] private Animator platformLaunchAnimator;
    [SerializeField] private Mouse mouse;
    private float idleToPlatformAnimLength;
    private float idleToCurveAnimLength;
    private float curveToPlatformAnimLength;
    private float curveToIdleAnimLength;
    private float platformToCurveAnimLength;
    private float platformToIdleAnimLength;
    private float elevatorLaunchAnimClipLength;
    private Vector3 platformPositionBeforeLaunch;
    private readonly Vector3 platformLaunchVerticalBoost = new Vector3(0, 15, 0);
    private bool isMouseSticked = false;
    private Transform mouseParentBeforeStick;
    private Transform mouseTransform;
    private Rigidbody2D mouseRigidbody;

    protected override void Awake()
    {
        base.Awake();

        this.jumpHeight = 1;
        this.movementSpeed = 1.5f;
        idleToPlatformAnimLength = platformAnimClip.length;
        idleToCurveAnimLength = curveAnimClip.length;
        platformToCurveAnimLength = platformToCurveAnimClip.length;
        platformToIdleAnimLength = platformToIdleAnimClip.length;
        curveToPlatformAnimLength = curveToPlatformAnimClip.length;
        curveToIdleAnimLength = curveToIdleAnimClip.length;
        elevatorLaunchAnimClipLength = launchPlatformAnimClip.length;
        mouseTransform = mouse.GetComponent<Transform>();
        mouseRigidbody = mouse.GetComponent<Rigidbody2D>();
    }

    public void AnimateIdleToPlatform()
    {
        StartCoroutine(HelperFunctions.CoRunMethodWithParamAfterSeconds(SetIsAnimating, false, idleToPlatformAnimLength));

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(platformColliderObj, idleToPlatformAnimLength));
        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(eleHoldingPlatformColliderObj, idleToPlatformAnimLength));

        animator.SetBool("ElephantDoingPlatform", true);
        animator.SetBool("ElephantDoingCurve", false);

        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(idleColliderObj, idleToPlatformAnimLength));
    }
    public void AnimatePlatformToIdle()
    {
        StartCoroutine(HelperFunctions.CoRunMethodWithParamAfterSeconds(SetIsAnimating, false, platformToIdleAnimLength));

        platformColliderObj.enabled = false;

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(idleColliderObj, platformToIdleAnimLength));

        animator.SetBool("ElephantDoingPlatform", false);

        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(eleHoldingPlatformColliderObj, platformToIdleAnimLength));
    }

    public void AnimateIdletoCurve()
    {
        StartCoroutine(HelperFunctions.CoRunMethodWithParamAfterSeconds(SetIsAnimating, false, idleToCurveAnimLength));

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(curveColliderObj, idleToCurveAnimLength));

        animator.SetBool("ElephantDoingCurve", true);
        animator.SetBool("ElephantDoingPlatform", false);

        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(idleColliderObj, idleToCurveAnimLength));
    }

    public void AnimateCurveToIdle()
    {
        StartCoroutine(HelperFunctions.CoRunMethodWithParamAfterSeconds(SetIsAnimating, false, curveToIdleAnimLength));

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(idleColliderObj, curveToIdleAnimLength));

        animator.SetBool("ElephantDoingCurve", false);

        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(curveColliderObj, curveToIdleAnimLength));
    }

    public void AnimateCurveToPlatform()
    {
        StartCoroutine(HelperFunctions.CoRunMethodWithParamAfterSeconds(SetIsAnimating, false, curveToPlatformAnimLength));

        animator.SetBool("ElephantDoingPlatform", true);

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(platformColliderObj, curveToPlatformAnimLength));
        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(eleHoldingPlatformColliderObj, curveToPlatformAnimLength));
        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(curveColliderObj, curveToPlatformAnimLength));

        animator.SetBool("ElephantDoingCurve", false);
    }

    public void AnimatePlatformToCurve()
    {
        StartCoroutine(HelperFunctions.CoRunMethodWithParamAfterSeconds(SetIsAnimating, false, platformToCurveAnimLength));

        platformColliderObj.enabled = false;

        animator.SetBool("ElephantDoingCurve", true);

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(curveColliderObj, platformToCurveAnimLength));
        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(eleHoldingPlatformColliderObj, platformToCurveAnimLength));

        animator.SetBool("ElephantDoingPlatform", false);

    }

    public void LaunchPlatform(Mouse mouse)
    {
        platformPositionBeforeLaunch = platformColliderObj.transform.position;

        if (IsMouseIsOnPlatform(mouse) && mouse != null)
        {
            CheckAndFreeMouseFromPlatform();
            StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(platformColliderObj, elevatorLaunchAnimClipLength));
            StartCoroutine(HelperFunctions.CoChangePositionAfterSeconds(platformColliderObj.transform, platformPositionBeforeLaunch, elevatorLaunchAnimClipLength));
            platformLaunchAnimator.SetTrigger("LaunchPlatform");
            mouseRigidbody.AddForce(platformLaunchVerticalBoost, ForceMode2D.Impulse);
        }
    }

    public void CheckAndStickMouseToPlatform(Mouse mouse)
    {
        if (IsMouseIsOnPlatform(mouse))
        {
            mouseParentBeforeStick = mouse.transform.parent;
            mouseTransform.transform.SetParent(transform, worldPositionStays: true);
            isMouseSticked = true;
        }
    }

    public void CheckAndFreeMouseFromPlatform()
    {
        if (isMouseSticked)
        {
            mouseTransform.transform.SetParent(mouseParentBeforeStick);
            isMouseSticked = false;
        }
    }

    private bool IsMouseIsOnPlatform(Mouse mouse)
    {
        var mouseCollider = mouse.GetComponent<BoxCollider2D>();

        if (mouseCollider.IsTouching(platformColliderObj))
        {
            return true;
        }

        return false;
    }
}
