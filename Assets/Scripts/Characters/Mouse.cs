using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Mouse : Character
{
    [HideInInspector] public bool isOnMount;
    [HideInInspector] public bool isTimeSlowed;
    [HideInInspector] public bool firstTrampolineJumpDone;

    [SerializeField] private Transform elephantSeatTransform;
    [SerializeField] private Animator slowTimeAnimator;
    private readonly float mountingTime = 0.6f;
    private readonly float mass = 1.0f;
    private readonly float seatAvaiableRange = 12.0f;
    private readonly float slowTimeScale = 0.5f;
    private readonly int defaultTimeScale = 1;
    private float slowTimeSpeedBonus;
    private Transform mouseParentBeforeMount;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private LayerMask trunkLayerMask;

    protected override void Awake()
    {
        base.Awake();

        this.jumpHeight = 1.7f;
        this.movementSpeed = 2f;
        mouseParentBeforeMount = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        trunkLayerMask = 1 << LayerMask.NameToLayer(Helpers.TrunkLayerName);
        slowTimeSpeedBonus = defaultTimeScale / slowTimeScale;
    }

    private void Update()
    {
        print("mouse velocity.y: " + rb.velocity.y.ToString());
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
        isTimeSlowed = true;
        Time.timeScale = slowTimeScale;
        slowTimeAnimator.SetTrigger("SlowTime");
        this.movementSpeed *= slowTimeSpeedBonus;
    }

    public void BackToDefaultTime()
    {
        isTimeSlowed = false;
        Time.timeScale = defaultTimeScale;
        slowTimeAnimator.SetTrigger("BackToDefaultTime");
        this.movementSpeed /= slowTimeSpeedBonus;
    }

    public IEnumerator CoWaitUntilMouseStopsJumpingOnTrampoline()
    {
        while (!IsMouseOnTrampoline())
        {
            yield return null;
        }

        firstTrampolineJumpDone = false;
    }

    private bool IsMouseOnTrampoline()
    {
        return isCharacterGrounded && groundCheck.boxCollider.IsTouchingLayers(trunkLayerMask);
    }

    private IEnumerator CoMountTheElephant()
    {
        float timePassed = 0;
        float lerpFactor;
        float deltaTime = 0.005f;
        Vector2 initialPosition = transform.position;
        Vector2 targetPosition = elephantSeatTransform.position;
        var waitDeltaTime = new WaitForSeconds(deltaTime);

        float arcHeight = 2.0f;

        while (timePassed < mountingTime)
        {
            targetPosition = elephantSeatTransform.position;

            lerpFactor = timePassed / mountingTime;

            Vector2 linearPosition = Vector2.Lerp(initialPosition, targetPosition, lerpFactor);

            float height = Mathf.Sin(Mathf.PI * lerpFactor) * arcHeight;
            Vector2 arcPosition = new Vector2(linearPosition.x, linearPosition.y + height);

            transform.position = arcPosition;

            yield return waitDeltaTime;
            timePassed += deltaTime;
        }

        transform.position = targetPosition;
    }
}
