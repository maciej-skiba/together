public static class Helpers
{
    public static string TrunkLayerName = "Trunk";
    public static string GroundLayerName = "Ground";
    public static string ElephantLayerName = "Elephant";
    public static string ElephantTopLayerName = "ElephantTop";
    public static string MouseLayerName = "Mouse";
    public static string BallLayerName = "Ball";

    public enum Characters
    {
        Elephant,
        Mouse
    }

    public enum Scenes
    {
        Menu = 0,
        Instruction,
        SelectLevel,
        FirstLevel,
        SecondLevel,
        ThirdLevel,
        End,
        Credits,
        Credits2
    }
}
