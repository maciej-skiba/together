using System.Linq;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private Mouse mouse;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Helpers.MouseLayerName))
        {
            checkpoint.TeleportCharacter(mouse.transform);
        }
    }
}
