using UnityEngine;

public class Character : MonoBehaviour
{
    public static HelperStructures.Characters currentCharacter;

    [HideInInspector] public float jumpHeight;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public bool isCharacterGrounded = true;

    protected Animator animator;

    [SerializeField] private GroundCheck groundCheck;
    private bool spriteFacingRight = true;
    private Quaternion facingRightRotation = new Quaternion(0, 0, 0, 0);
    private Quaternion facingLeftRotation = new Quaternion(0, 180, 0, 0);

    protected virtual void Awake()
    {
        currentCharacter = HelperStructures.Characters.Elephant;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isCharacterGrounded)
        {
            CheckIfGrounded();
        }
    }

    public void CheckAndFlipCharacter(bool movingRight)
    {
        if (movingRight)
        {
            if (!spriteFacingRight)
            {
                transform.rotation = facingRightRotation;
                spriteFacingRight = true;
            }
        }
        else
        {
            if (spriteFacingRight)
            {
                transform.rotation = facingLeftRotation;
                spriteFacingRight = false;
            }
        }
    }

    private void CheckIfGrounded()
    {
        if (groundCheck.IsGrounded())
        {
            isCharacterGrounded = true;
        }
    }
}
