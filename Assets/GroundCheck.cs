using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Character character;
    private bool isGrounded;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Ground"))
        {
            isGrounded = true;
            character.isCharacterGrounded = isGrounded;
        }

        print("is grounded:" + isGrounded.ToString());
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Ground"))
        {
            isGrounded = false;
            character.isCharacterGrounded = isGrounded;
        }

        print("is grounded:" + isGrounded.ToString());
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
