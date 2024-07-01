using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : Character
{
    [HideInInspector] public bool isOnMount;
    [HideInInspector] public bool isTimeSlowed;
    [HideInInspector] public bool firstTrampolineJumpDone;

    [SerializeField] private Transform elephantSeatTransform;
    [SerializeField] private Image[] slowTimeBarElements;
    [SerializeField] private Animator slowTimeAnimator;
    private readonly float mountingTime = 0.4f;
    private readonly float mass = 1.0f;
    private readonly float seatAvaiableRange = 12.0f;
    private readonly float slowTimeScale = 0.5f;
    private readonly int defaultTimeScale = 1;
    private readonly float maxSlowTime = 4.0f;
    private float slowTimeFactor;
    private float remainingSlowTime;
    private Transform mouseParentBeforeMount;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private LayerMask trunkLayerMask;

    protected override void Awake()
    {
        base.Awake();

        remainingSlowTime = maxSlowTime;
        this.jumpHeight = 1.7f;
        this.movementSpeed = 2f;
        mouseParentBeforeMount = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        trunkLayerMask = 1 << LayerMask.NameToLayer(Helpers.TrunkLayerName);
        slowTimeFactor = defaultTimeScale / slowTimeScale;
    }

    protected override void Update()
    {
        base.Update();

        if (isTimeSlowed)
        {
            remainingSlowTime -= Time.deltaTime * slowTimeFactor;

            if (remainingSlowTime <= 0)
            {
                BackToDefaultTime();
                MakeSlowTimeBarTransparent();
            }
        }
        else if (!isTimeSlowed && remainingSlowTime < maxSlowTime)
        {
            remainingSlowTime += Time.deltaTime;
        }
        else if (remainingSlowTime > maxSlowTime)
        {
            remainingSlowTime = maxSlowTime;
        }
        else return;

        foreach (var element in slowTimeBarElements)
        {
            element.fillAmount = remainingSlowTime / maxSlowTime;
        }
    }

    public void MountTheElephant()
    {
        if (Vector2.Distance(elephantSeatTransform.position, transform.position) > seatAvaiableRange)
        {
            SetIsAnimating(false);
            return;
        }

        StartCoroutine(CoMountTheElephant());
        boxCollider.enabled = false;
        isOnMount = true;
        StartCoroutine(HelperFunctions.CoRunMethodWithParamAfterSeconds(SetIsAnimating, false, mountingTime));
        mouseParentBeforeMount = transform.parent;
        transform.SetParent(elephantSeatTransform, worldPositionStays: true);
        rb.isKinematic = true;
        rb.mass = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void UnMountTheElephant()
    {
        boxCollider.enabled = true;
        isOnMount = false;
        transform.SetParent(mouseParentBeforeMount, worldPositionStays: true);
        rb.isKinematic = false;
        rb.mass = mass;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SlowTime()
    {
        foreach (var element in slowTimeBarElements)
        {
            StartCoroutine(HelperFunctions.CoChangeImageAlpha(element, 0.3f, alpha: 1.0f)); 
        }

        isTimeSlowed = true;
        Time.timeScale = slowTimeScale;
        slowTimeAnimator.SetTrigger("SlowTime");
        this.movementSpeed *= slowTimeFactor;
        InputManager.Instance.ReloadMovementProperties();
    }

    public void BackToDefaultTime()
    {
        isTimeSlowed = false;
        Time.timeScale = defaultTimeScale;
        slowTimeAnimator.SetTrigger("BackToDefaultTime");
        this.movementSpeed /= slowTimeFactor;
        InputManager.Instance.ReloadMovementProperties();
    }

    public IEnumerator CoWaitUntilMouseStopsJumpingOnTrampoline()
    {
        while (!IsMouseOnTrampoline())
        {
            yield return null;
        }

        firstTrampolineJumpDone = false;
    }

    public void HideSlowTimeBar()
    {
        foreach (var element in slowTimeBarElements)
        {
            StartCoroutine(HelperFunctions.CoHideImage(img: element, timeInSeconds: 0.3f));
        }
    }

    public void MakeSlowTimeBarTransparent()
    {
        foreach (var element in slowTimeBarElements)
        {
            StartCoroutine(HelperFunctions.CoChangeImageAlpha(img: element, timeInSeconds: 0.3f, alpha: 0.25f));
        }
    }

    public void ShowSlowTimeBar()
    {
        foreach (var element in slowTimeBarElements)
        {
            StartCoroutine(HelperFunctions.CoShowImage(img: element, timeInSeconds: 0.3f));
        }
    }

    private bool IsMouseOnTrampoline()
    {
        return isCharacterGrounded && groundCheck.boxCollider.IsTouchingLayers(trunkLayerMask);
    }

    private IEnumerator CoMountTheElephant()
    {
        float timePassed = 0;
        float lerpFactor;
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition = elephantSeatTransform.position;

        float arcHeight = 2.0f;

        while (timePassed < mountingTime)
        {
            targetPosition = elephantSeatTransform.position;

            lerpFactor = timePassed / mountingTime;

            Vector2 linearPosition = Vector2.Lerp(initialPosition, targetPosition, lerpFactor);

            float height = Mathf.Sin(Mathf.PI * lerpFactor) * arcHeight;
            Vector2 arcPosition = new Vector2(linearPosition.x, linearPosition.y + height);

            transform.position = arcPosition;

            timePassed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.position = targetPosition;
    }
}
