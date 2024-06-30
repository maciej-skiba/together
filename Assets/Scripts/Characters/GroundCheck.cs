using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public BoxCollider2D boxCollider;

    [SerializeField] private Character character;
    private bool isGrounded;
    private int groundLayer;
    private int trunkLayer;
    private int elephantTopLayer;
    private int groundingLayersCombined;
    private Rigidbody2D characterRigidbody;

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer(Helpers.GroundLayerName);
        trunkLayer = LayerMask.NameToLayer(Helpers.TrunkLayerName);
        elephantTopLayer = LayerMask.NameToLayer(Helpers.ElephantTopLayerName);
        groundingLayersCombined = 
              (1 << groundLayer)
            + (1 << trunkLayer)
            + (1 << elephantTopLayer);

        characterRigidbody = character.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        var obj = collider.gameObject;

        if (obj.layer == elephantTopLayer)
        {
            isGrounded = true;
            character.isCharacterGrounded = isGrounded;
        }

        if (Math.Abs(characterRigidbody.velocity.y) > 0.05f) return;

        if (((1 << obj.layer) & groundingLayersCombined) > 0)
        {
            isGrounded = true;
            character.isCharacterGrounded = isGrounded;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var obj = collider.gameObject;

        if (((1 << obj.layer) & groundingLayersCombined) > 0)
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
