using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public BoxCollider2D boxCollider;

    [SerializeField] private Character character;
    private bool isGrounded;
    private int groundLayer;
    private int trunkLayer;
    private int elephantLayer;
    private int groundingLayersCombined;
    private Rigidbody2D characterRigidbody;

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer(Helpers.GroundLayerName);
        trunkLayer = LayerMask.NameToLayer(Helpers.TrunkLayerName);
        elephantLayer = LayerMask.NameToLayer(Helpers.ElephantLayerName);
        groundingLayersCombined = groundLayer + trunkLayer + elephantLayer;

        characterRigidbody = character.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (characterRigidbody.velocity.y > 0) return;

        var obj = collider.gameObject;

        if ((obj.layer & groundingLayersCombined) > 0)
        {
            isGrounded = true;
            character.isCharacterGrounded = isGrounded;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var obj = collider.gameObject;

        if ((obj.layer & groundingLayersCombined) > 0)
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
