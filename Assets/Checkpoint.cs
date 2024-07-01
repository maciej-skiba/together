using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void TeleportCharacter(Transform characterTransform)
    {
        characterTransform.position = transform.position;
    }
}
