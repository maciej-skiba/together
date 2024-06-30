using UnityEngine;

public class SmallButton : StandingButton
{
    private void Awake()
    {
        layersTriggeringButton =
            (1 << LayerMask.NameToLayer(Helpers.ElephantLayerName))
            + (1 << LayerMask.NameToLayer(Helpers.MouseLayerName))
            + (1 << LayerMask.NameToLayer(Helpers.BallLayerName));
    }
}
