using UnityEngine;

public class BigButton : StandingButton
{
    private void Awake()
    {
        layersTriggeringButton =
            (1 << LayerMask.NameToLayer(Helpers.ElephantLayerName))
            + (1 << LayerMask.NameToLayer(Helpers.BallLayerName));
    }
}
