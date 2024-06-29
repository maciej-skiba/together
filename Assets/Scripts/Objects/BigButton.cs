using UnityEngine;

public class BigButton : MonoBehaviour
{
    [SerializeField] private Mechanism mechanism;
    static private LayerMask layerTriggeringButton;

    static private void Awake()
    {
        layerTriggeringButton = LayerMask.NameToLayer(Helpers.ElephantLayerName);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == layerTriggeringButton)
        {
            mechanism.On();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == layerTriggeringButton)
        {
            mechanism.Off();
        }
    }
}
