using UnityEngine;
using Cinemachine.Utility;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Elephant elephant;
    [SerializeField] private Mouse mouse;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private GameObject currentGameObject;
    private Rigidbody currentRigidbody;
    private float currentJumpHeight;
    private float currentMovementSpeed;
    private Vector3 currentJumpVector;
    private Vector3 currentMovementSpeedVector;

    private void Start()
    {
        currentGameObject = elephant.gameObject;
        currentRigidbody = elephant.GetComponent<Rigidbody>();
        currentMovementSpeed = elephant.movementSpeed;
        currentMovementSpeedVector = new Vector3(currentMovementSpeed, 0, 0);
        currentJumpHeight = elephant.jumpHeight;
        currentJumpVector = new Vector3(0, currentJumpHeight, 0);
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
            currentRigidbody.AddForce(currentJumpVector, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            currentGameObject.transform.position -= currentMovementSpeedVector; 
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentGameObject.transform.position += currentMovementSpeedVector;
        }


        // Swap characters

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SwapCharacters();
        }
    }

    private GameObject ReturnCurrentCharacterGameObject()
    {
        return Character.currentCharacter == HelperStructures.Characters.Elephant ? elephant.gameObject : mouse.gameObject;
    }

    private void SwapCharacters()
    {
        if (Character.currentCharacter == HelperStructures.Characters.Elephant)
        {
            Character.currentCharacter = HelperStructures.Characters.Mouse;
            currentGameObject = mouse.gameObject;
            currentMovementSpeed = mouse.movementSpeed;
            currentRigidbody = mouse.GetComponent<Rigidbody>();
            currentJumpHeight = mouse.jumpHeight;
        }
        else
        {
            Character.currentCharacter = HelperStructures.Characters.Elephant;
            currentGameObject = elephant.gameObject;
            currentMovementSpeed = elephant.movementSpeed;
            currentRigidbody = elephant.GetComponent<Rigidbody>();
            currentJumpHeight = elephant.jumpHeight;
        }

        currentJumpVector = new Vector3(0, currentJumpHeight, 0);
        currentMovementSpeedVector = new Vector3(currentMovementSpeed, 0, 0);
        virtualCamera.Follow = currentGameObject.transform;
    }
}
