using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Character character;
    private bool isGrounded;
    private int groundLayer;
    private int trunkLayer;

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        trunkLayer = LayerMask.NameToLayer("Trunk");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var obj = collider.gameObject;

        if (obj.layer == groundLayer || obj.layer == trunkLayer)
        {
            isGrounded = true;
            character.isCharacterGrounded = isGrounded;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var obj = collider.gameObject;

        if (obj.layer == groundLayer || obj.layer == trunkLayer)
        {
            isGrounded = false;
            character.isCharacterGrounded = isGrounded;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
