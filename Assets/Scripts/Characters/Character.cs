using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Helpers.Characters currentCharacter;

    public AudioSource actionOneSound;
    public AudioSource actionTwoSound;

    [HideInInspector] public float jumpHeight;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public bool isCharacterGrounded = true;
    [HideInInspector] public bool isAnimating = false;

    [SerializeField] protected GroundCheck groundCheck;
    protected Animator animator;

    private bool spriteFacingRight = true;
    private Quaternion facingRightRotation = new Quaternion(0, 0, 0, 0);
    private Quaternion facingLeftRotation = new Quaternion(0, 180, 0, 0);

    protected virtual void Awake()
    {
        currentCharacter = Helpers.Characters.Elephant;
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (!isCharacterGrounded)
        {
            CheckIfGrounded();
        }
    }
    public IEnumerator CoFreezeCharacterIfGrounded(Rigidbody2D rb)
    {
        var waitTimeInSeconds = new WaitForSeconds(0.05f);

        while (!isCharacterGrounded)
        {
            yield return waitTimeInSeconds;
        }

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void CheckAndFlipCharacter(bool movingRight, Vector3 repositionVector = default(Vector3))
    {
        if (movingRight)
        {
            if (!spriteFacingRight)
            {
                transform.position += repositionVector;
                transform.rotation = facingRightRotation;
                spriteFacingRight = true;
            }
        }
        else
        {
            if (spriteFacingRight)
            {
                transform.position -= repositionVector;
                transform.rotation = facingLeftRotation;
                spriteFacingRight = false;
            }
        }
    }

    public void SetIsAnimating(bool value = true)
    {
        isAnimating = value;
    }

    private void CheckIfGrounded()
    {
        if (groundCheck.IsGrounded())
        {
            isCharacterGrounded = true;
        }
    }
}
