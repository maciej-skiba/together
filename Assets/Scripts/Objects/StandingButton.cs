using UnityEngine;

public class StandingButton : MonoBehaviour
{
    protected LayerMask layersTriggeringButton;
    
    [SerializeField] private Mechanism[] mechanisms;

    // to handle ball on button
    protected void OnTriggerStay2D(Collider2D collider)
    {
        var colliderLayerMask = 1 << collider.gameObject.layer;

        if ((colliderLayerMask & layersTriggeringButton) > 0)
        {
            foreach (var mechanism in mechanisms)
            {
                mechanism.On();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var colliderLayerMask = 1 << collider.gameObject.layer;

        if ((colliderLayerMask & layersTriggeringButton) > 0)
        {
            foreach (var mechanism in mechanisms)
            {
                mechanism.Off();
            }
        }
    }


}
