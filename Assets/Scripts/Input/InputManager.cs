using UnityEngine;
using Cinemachine.Utility;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Elephant elephant;
    [SerializeField] private Mouse mouse;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private GameObject currentGameObject;
    private Rigidbody2D currentRigidbody;
    private float currentJumpHeight;
    private float currentMovementSpeed;
    private float jumpFactor = 6f;
    private float movementSpeedFactor = 0.03f;
    private Vector2 currentJumpVector;
    private Vector3 currentMovementSpeedVector;

    private void Start()
    {
        currentGameObject = elephant.gameObject;
        currentRigidbody = elephant.GetComponent<Rigidbody2D>();
        currentMovementSpeed = elephant.movementSpeed;
        currentMovementSpeedVector = new Vector3(currentMovementSpeed * movementSpeedFactor, 0, 0);
        currentJumpHeight = elephant.jumpHeight;
        currentJumpVector = new Vector2(0, currentJumpHeight * jumpFactor);

        mouse.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

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

    private GameObject ReturnCurrentCharacterGameObject()
    {
        return Character.currentCharacter == HelperStructures.Characters.Elephant ? elephant.gameObject : mouse.gameObject;
    }

    private void SwapCharacters()
    {
        currentRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        if (Character.currentCharacter == HelperStructures.Characters.Elephant)
        {
            Character.currentCharacter = HelperStructures.Characters.Mouse;
            currentGameObject = mouse.gameObject;
            currentMovementSpeed = mouse.movementSpeed;
            currentRigidbody = mouse.GetComponent<Rigidbody2D>();
            currentJumpHeight = mouse.jumpHeight;
        }
        else
        {
            Character.currentCharacter = HelperStructures.Characters.Elephant;
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

        if (Character.currentCharacter == HelperStructures.Characters.Elephant)
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

        if (Character.currentCharacter == HelperStructures.Characters.Elephant)
        {
            elephant.CheckAndFlipCharacter(movingRight: true);
        }
        else
        {
            mouse.CheckAndFlipCharacter(movingRight: true);
        }
    }

    private void Jump()
    {
        if (Character.currentCharacter == HelperStructures.Characters.Elephant)
        {
            if (!elephant.isCharacterGrounded) return;

            elephant.isCharacterGrounded = false;
        }
        else 
        {
            if (!mouse.isCharacterGrounded) return;

            mouse.isCharacterGrounded = false;
        }

        currentRigidbody.AddForce(currentJumpVector, ForceMode2D.Impulse);
    }

    private void SpecialActionOne()
    {
        if (Character.currentCharacter == HelperStructures.Characters.Elephant)
        {
            elephant.DoPlatform();
        }
        else
        {
            mouse.UnknownAction();
        }
    }

    private void SpecialActionTwo()
    {
        if (Character.currentCharacter == HelperStructures.Characters.Elephant)
        {
            elephant.DoCurve();
        }
        else
        {
            mouse.UnknownAction();
        }
    }
}
