using System.Collections.Generic;
using UnityEngine;

public class Elephant : Character
{
    [HideInInspector] public bool elephantDoingPlatform;
    [HideInInspector] public bool elephantDoingCurve;
    [HideInInspector] public bool isHoldingPlatform = false;
    [HideInInspector] public bool isHoldingBall = false;
    [HideInInspector] public Vector3 repositionAfterFlipVector;

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
    [SerializeField] private Transform ballHoldingPoint;
    [SerializeField] private GameObject ballsContainer;
    [SerializeField] private Transform elephantTopContainer;
    private List<BoxCollider2D> elephantTopBoxColliders;
    private List<GameObject> balls;
    private float idleToPlatformAnimLength;
    private float idleToCurveAnimLength;
    private float curveToPlatformAnimLength;
    private float curveToIdleAnimLength;
    private float platformToCurveAnimLength;
    private float platformToIdleAnimLength;
    private float elevatorLaunchAnimClipLength;
    private readonly Vector3 platformLaunchVerticalBoost = new Vector3(0, 15, 0);
    private Vector3 platformPositionBeforeLaunch;
    private bool isMouseSticked = false;
    private bool ElephantTopCollidable;
    private Transform mouseParentBeforeStick;
    private Transform mouseTransform;
    private Transform attachedBall;
    private Rigidbody2D mouseRigidbody;
    private Rigidbody2D attachedBallRigidbody;

    protected override void Awake()
    {
        base.Awake();

        this.jumpHeight = 1f;
        this.movementSpeed = 1.4f;
        idleToPlatformAnimLength = platformAnimClip.length;
        idleToCurveAnimLength = curveAnimClip.length;
        platformToCurveAnimLength = platformToCurveAnimClip.length;
        platformToIdleAnimLength = platformToIdleAnimClip.length;
        curveToPlatformAnimLength = curveToPlatformAnimClip.length;
        curveToIdleAnimLength = curveToIdleAnimClip.length;
        elevatorLaunchAnimClipLength = launchPlatformAnimClip.length;
        mouseTransform = mouse.GetComponent<Transform>();
        mouseRigidbody = mouse.GetComponent<Rigidbody2D>();
        repositionAfterFlipVector = new Vector3(2.0f, 0, 0);
        balls = new List<GameObject>();
        elephantTopBoxColliders = new List<BoxCollider2D>();

        for (int i = 0; i < ballsContainer.transform.childCount; i++)
        {
            balls.Add(ballsContainer.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < elephantTopContainer.childCount; i++)
        {
            Transform child = elephantTopContainer.transform.GetChild(i);
            elephantTopBoxColliders.Add(child.GetComponent<BoxCollider2D>());
        }
    }

    private void Update()
    {
        if (Character.currentCharacter != Helpers.Characters.Mouse) return;

        if (mouseRigidbody.velocity.y <= 0.1f && !ElephantTopCollidable)
        {
            MakeElephantTopCollidable();
        }
        else if (mouseRigidbody.velocity.y > 0.1f && ElephantTopCollidable)
        {
            MakeElephantTopNotCollidable();
        }
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

        idleColliderObj.enabled = true;
        platformColliderObj.enabled = false;

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(idleColliderObj, platformToIdleAnimLength));

        animator.SetBool("ElephantDoingPlatform", false);

        eleHoldingPlatformColliderObj.enabled = false;
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

        idleColliderObj.enabled = true;

        animator.SetBool("ElephantDoingCurve", false);

        curveColliderObj.enabled = false;
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

        animator.SetBool("ElephantDoingCurve", true);

        StartCoroutine(HelperFunctions.CoEnableCollider2DAfterSeconds(curveColliderObj, platformToCurveAnimLength));
        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(eleHoldingPlatformColliderObj, platformToCurveAnimLength));

        animator.SetBool("ElephantDoingPlatform", false);

    }

    //public void LaunchPlatform(Mouse mouse)
    //{
    //    platformPositionBeforeLaunch = platformColliderObj.transform.position;
    //    float rangeToLaunchMouse = 2f;

    //    if (HelperFunctions.IsLayerInRange(Vector2.down, Helpers.TrunkLayerName, rangeToLaunchMouse, mouse.gameObject))
    //    {
    //        CheckAndFreeMouseFromPlatform();
    //        StartCoroutine(HelperFunctions.CoDisableCollider2DAfterSeconds(platformColliderObj, elevatorLaunchAnimClipLength));
    //        StartCoroutine(HelperFunctions.CoChangePositionAfterSeconds(platformColliderObj.transform, platformPositionBeforeLaunch, elevatorLaunchAnimClipLength));
    //        platformLaunchAnimator.SetTrigger("LaunchPlatform");
    //        mouseRigidbody.AddForce(platformLaunchVerticalBoost, ForceMode2D.Impulse);
    //    }
    //    else
    //    {
    //        platformColliderObj.enabled = false;
    //    }
    //}
    public void GrabTheBall()
    {
        if (balls.Count == 0) return;

        if (!isHoldingBall)
        {
            var nearestBall = GetNearestBall();

            if (IsBallInRange(nearestBall.transform))
            {
                AttachTheBallToElephant(nearestBall.transform);
            }
        }
    }

    public void ReleaseTheBall()
    {
        if (balls.Count == 0) return;

        if (isHoldingBall)
        {
            DetachTheBallFromElephant(attachedBall);
        }
    }

    public void CheckAndStickMouseToPlatform(Mouse mouse)
    {
        float distanceRequiredToStick = 0.1f;

        if (HelperFunctions.IsLayerInRange(Vector2.down, Helpers.TrunkLayerName, distanceRequiredToStick, mouse.gameObject))
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

    private void MakeElephantTopCollidable()
    {
        foreach(var collider in elephantTopBoxColliders)
        {
            collider.enabled = true;
        }

        ElephantTopCollidable = true;
    }
    private void MakeElephantTopNotCollidable()
    {
        foreach (var collider in elephantTopBoxColliders)
        {
            collider.enabled = false;
        }
        
        ElephantTopCollidable = false;
    }


    private GameObject GetNearestBall()
    {
        GameObject nearestBall = balls[0];
        float distanceToCurrentBall;
        float smallestDistance = 0;
        
        for (int i = 0; i < balls.Count; i++)
        {
            if (i == 0)
            {
                smallestDistance = Vector2.Distance(balls[i].transform.position, ballHoldingPoint.position);
                nearestBall = balls[i];
            }
            else
            {
                distanceToCurrentBall = Vector2.Distance(balls[i].transform.position, ballHoldingPoint.position);

                if (smallestDistance > distanceToCurrentBall)
                {
                    smallestDistance = distanceToCurrentBall;
                    nearestBall = balls[i];
                }
            }
        }

        return nearestBall;
    }

    private bool IsBallInRange(Transform ball)
    {
        return HelperFunctions.IsObjectInRange(ballHoldingPoint.position, ball.position, range: 5.0f);
    }

    private void AttachTheBallToElephant(Transform ball)
    {
        HelperFunctions.ChangePosition(ball, ballHoldingPoint.transform.position);
        ball.SetParent(transform, worldPositionStays: true);
        attachedBall = ball;
        attachedBallRigidbody = attachedBall.GetComponent<Rigidbody2D>();
        attachedBallRigidbody.velocity = Vector2.zero;
        attachedBallRigidbody.isKinematic = true;
        isHoldingBall = true;
    }

    private void DetachTheBallFromElephant(Transform ball)
    {
        ball.SetParent(ballsContainer.transform, worldPositionStays: true);
        attachedBallRigidbody.isKinematic = false;
        attachedBallRigidbody = null;
        attachedBall = null;
        isHoldingBall = false;
    }
}
