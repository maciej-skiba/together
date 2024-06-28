public class Mouse : Character
{
    protected override void Awake()
    {
        base.Awake();

        this.jumpHeight = 1.7f;
        this.movementSpeed = 1.7f;
    }

    public void UnknownActionOne()
    {
        print("Played mouse's unknown action 1");
        isAnimating = false;
    }

    public void UnknownActionTwo()
    {
        print("Played mouse's unknown action 2");
        isAnimating = false;
    }
}
