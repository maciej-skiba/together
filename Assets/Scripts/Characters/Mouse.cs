public class Mouse : Character
{
    public void UnknownAction()
    {
        print("Played mouse's unknown action");
    }

    protected override void Awake()
    {
        base.Awake();

        this.jumpHeight = 1.7f;
        this.movementSpeed = 1.7f;
    }
}
