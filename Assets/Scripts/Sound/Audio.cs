using UnityEngine;

public class Audio : MonoBehaviour
{
    protected static readonly int defaultVolumeValue = 100;
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        if (gameObject != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
}
