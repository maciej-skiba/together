using UnityEngine;
using Cinemachine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Elephant elephant;
    [SerializeField] private Mouse mouse;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private GameObject currentGameObject;
    private Rigidbody2D currentRigidbody;
    private Rigidbody2D elephantRigidbody;
    private float currentJumpHeight;
    private float currentMovementSpeed;
    private float jumpFactor = 9f;
    private float movementSpeedFactor = 0.02f;
    private Vector2 currentJumpVector;
    private Vector3 currentMovementSpeedVector;
    private bool firstTrampolineJumpDone = false;
    private bool trampolineJumpAvailable = true;

    private void Start()
    {
        currentGameObject = elephant.gameObject;
        currentRigidbody = elephant.GetComponent<Rigidbody2D>();
        currentMovementSpeed = elephant.movementSpeed;
        currentMovementSpeedVector = new Vector3(currentMovementSpeed * movementSpeedFactor, 0, 0);
        currentJumpHeight = elephant.jumpHeight;
        currentJumpVector = new Vector2(0, currentJumpHeight * jumpFactor);

        mouse.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        elephantRigidbody = elephant.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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

        // Jumping / Moving

        if (!(mouse.isOnMount && Character.currentCharacter == Helpers.Characters.Mouse))
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                TrampolineJump();
            }

            if (firstTrampolineJumpDone)
            {
                StartCoroutine(mouse.CoIsMouseJumpingOnTrampoline());
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

    private void SwapCharacters()
    {
        currentRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            StartCoroutine(mouse.CoFreezeCharacterIfGrounded(elephantRigidbody));
            elephant.CheckAndFreeMouseFromPlatform();

            Character.currentCharacter = Helpers.Characters.Mouse;
            currentGameObject = mouse.gameObject;
            currentMovementSpeed = mouse.movementSpeed;
            currentRigidbody = mouse.GetComponent<Rigidbody2D>();
            currentJumpHeight = mouse.jumpHeight;
        }
        else
        {
            elephant.CheckAndStickMouseToPlatform(mouse);

            Character.currentCharacter = Helpers.Characters.Elephant;
            currentGameObject = elephant.gameObject;
            currentMovementSpeed = elephant.movementSpeed;
            currentRigidbody = elephant.GetComponent<Rigidbody2D>();
            currentJumpHeight = elephant.jumpHeight;
        }

        currentJumpVector = new Vector2(0, currentJumpHeight * jumpFactor);
        currentMovementSpeedVector = new Vector3(currentMovementSpeed * movementSpeedFactor, 0, 0);
        virtualCamera.Follow = currentGameObject.transform;

        currentRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void MoveLeft()
    {
        currentGameObject.transform.position -= currentMovementSpeedVector;

        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            elephant.CheckAndFlipCharacter(movingRight: false);
        }
        else
        {
            mouse.CheckAndFlipCharacter(movingRight: false);
        }
    }

    private void MoveRight()
    {
        currentGameObject.transform.position += currentMovementSpeedVector;

        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            elephant.CheckAndFlipCharacter(movingRight: true);
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
        }
        else 
        {
            if (!mouse.isCharacterGrounded) return;

            mouse.isCharacterGrounded = false;
        }

        currentRigidbody.AddForce(currentJumpVector * jumpBoost, ForceMode2D.Impulse);
    }

    private void TrampolineJump()
    {
        if (trampolineJumpAvailable)
        {
            if (Character.currentCharacter == Helpers.Characters.Mouse
                && HelperFunctions.IsLayerInRange(Vector2.down, "Trunk", 1.0f, mouse.gameObject))
            {
                trampolineJumpAvailable = false;

                if (!firstTrampolineJumpDone)
                {
                    Jump();
                    firstTrampolineJumpDone = true;
                }
                else
                {
                    Jump(jumpBoost: 1.2f);
                }
            }
            else
            {
                trampolineJumpAvailable = true;
            }
        }
    }
    private void SpecialActionOne()
    {
        if (Character.currentCharacter == Helpers.Characters.Elephant)
        {
            if (elephant.isAnimating) return;

            elephant.SetIsAnimating(true);

            if (elephant.isHoldingPlatform)
            {
                elephant.isHoldingPlatform = false;
                elephant.AnimatePlatformToIdle();
                elephant.CheckAndFreeMouseFromPlatform();
            }
            else if (elephant.isHoldingCurve)
            {
                elephant.isHoldingCurve = false;
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
            if (elephant.isAnimating) return;

            elephant.SetIsAnimating(true);

            if (elephant.isHoldingCurve)
            {
                elephant.isHoldingCurve = false;
                elephant.AnimateCurveToIdle();
            }
            else if (elephant.isHoldingPlatform)
            {
                elephant.isHoldingPlatform = false;
                elephant.AnimatePlatformToCurve();
                elephant.LaunchPlatform(mouse);
                elephant.isHoldingCurve = true;
            }
            else
            {
                elephant.isHoldingCurve= true;
                elephant.AnimateIdletoCurve();
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
            }
        }
    }
}
