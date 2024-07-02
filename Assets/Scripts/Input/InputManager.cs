using UnityEngine;
using Cinemachine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private Elephant elephant;
    [SerializeField] private Mouse mouse;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private GameObject currentGameObject;
    private Rigidbody2D currentRigidbody;
    private float currentJumpHeight;
    private float currentMovementSpeed;
    private float jumpFactor = 9f;
    private float movementSpeedFactor = 5f;
    private Vector2 currentJumpVector;
    private Vector3 currentMovementSpeedVector;
    private AudioSource elephantJump;
    private AudioSource mouseJump;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        currentGameObject = elephant.gameObject;
        currentRigidbody = elephant.GetComponent<Rigidbody2D>();
        currentMovementSpeed = elephant.movementSpeed;
        currentMovementSpeedVector = new Vector3(currentMovementSpeed * movementSpeedFactor, 0, 0);
        currentJumpHeight = elephant.jumpHeight;
        currentJumpVector = new Vector2(0, currentJumpHeight * jumpFactor);
        elephantJump = elephant.GetComponent<AudioSource>();
        mouseJump = mouse.GetComponent<AudioSource>();

        mouse.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        // Jumping / Moving

        if (!(mouse.isOnMount && Character.currentCharacter == Helpers.Characters.Mouse))
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft();
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight();
            }
        }

        // Pause

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Pause.gamePaused)
            {
                Pause.Instance.PauseOff();
            }
            else
            {
                Pause.Instance.PauseOn();
            }
        }

        // Swap characters

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SwapCharacters();
        }

        // Special actions

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            SpecialActionOne();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
        {
            SpecialActionTwo();
        }
    }

    public void ReloadMovementProperties()
    {
        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            currentMovementSpeed = elephant.movementSpeed;
        }
        else
        {
            currentMovementSpeed = mouse.movementSpeed;
        }

        currentJumpVector = new Vector2(0, currentJumpHeight * jumpFactor);
        currentMovementSpeedVector = new Vector3(currentMovementSpeed * movementSpeedFactor, 0, 0);
    }

    private void SwapCharacters()
    {
        currentRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            mouse.MakeSlowTimeBarTransparent();

            Character.currentCharacter = Helpers.Characters.Mouse;
            currentGameObject = mouse.gameObject;
            currentMovementSpeed = mouse.movementSpeed;
            currentRigidbody = mouse.GetComponent<Rigidbody2D>();
            currentJumpHeight = mouse.jumpHeight;
        }
        else
        {
            //elephant.CheckAndStickMouseToPlatform(mouse);
            if (mouse.isTimeSlowed) mouse.BackToDefaultTime();

            mouse.HideSlowTimeBar();

            Character.currentCharacter = Helpers.Characters.Elephant;
            currentGameObject = elephant.gameObject;
            currentMovementSpeed = elephant.movementSpeed;
            currentRigidbody = elephant.GetComponent<Rigidbody2D>();
            currentJumpHeight = elephant.jumpHeight;
        }

        ReloadMovementProperties();
        virtualCamera.Follow = currentGameObject.transform;

        currentRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void MoveLeft()
    {
        currentGameObject.transform.position -= currentMovementSpeedVector * Time.deltaTime;

        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            elephant.CheckAndFlipCharacter(movingRight: false, repositionVector: elephant.repositionAfterFlipVector);
        }
        else
        {
            mouse.CheckAndFlipCharacter(movingRight: false);
        }
    }

    private void MoveRight()
    {
        currentGameObject.transform.position += currentMovementSpeedVector * Time.deltaTime;

        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            elephant.CheckAndFlipCharacter(movingRight: true, repositionVector: elephant.repositionAfterFlipVector);
        }
        else
        {
            mouse.CheckAndFlipCharacter(movingRight: true);
        }
    }

    private void Jump(float jumpBoost = 1)
    {
        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            if (!elephant.isCharacterGrounded) return;

            elephant.isCharacterGrounded = false;
            elephantJump.Play();
        }
        else 
        {
            if (!mouse.isCharacterGrounded) return;

            mouse.isCharacterGrounded = false;
            mouseJump.Play();
        }

        currentRigidbody.AddForce(currentJumpVector * jumpBoost, ForceMode2D.Impulse);
    }

    private void SpecialActionOne()
    {
        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            if (elephant.isAnimating || elephant.isHoldingBall) return;

            elephant.SetIsAnimating(true);
            elephant.actionOneSound.Play();

            if (elephant.isHoldingPlatform)
            {
                elephant.isHoldingPlatform = false;
                elephant.AnimatePlatformToIdle();
                elephant.CheckAndFreeMouseFromPlatform();
            }
            else if (elephant.isHoldingBall)
            {
                elephant.isHoldingBall = false;
                elephant.AnimateCurveToPlatform();
                elephant.isHoldingPlatform = true;
            }
            else
            {
                elephant.isHoldingPlatform = true;
                elephant.AnimateIdleToPlatform();
            }
        }
        else
        {
            if (mouse.isAnimating) return;

            if (mouse.isOnMount)
            {
                mouse.UnMountTheElephant();
            }
            else
            {
                mouse.SetIsAnimating(true);
                mouse.MountTheElephant();
            }
        }
    }

    private void SpecialActionTwo()
    {
        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            if (elephant.isAnimating || elephant.isHoldingPlatform) return;

            if (elephant.isHoldingBall)
            {
                elephant.ReleaseTheBall();
                elephant.actionTwoSound.Play();
            }
            else
            {
                elephant.GrabTheBall();
            }
        }
        else
        {
            if (mouse.isTimeSlowed)
            {
                mouse.BackToDefaultTime();
            }
            else
            {
                mouse.SlowTime();
                mouse.actionTwoSound.Play();
            }
        }
    }
}
