using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance { get; private set; }
    private AudioSource audioSource;
    private string menuMusicName = "Menu";

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
            audioSource.Play();
            DontDestroyOnLoad(Instance.gameObject);
        }
        else if (Instance != null && audioSource.clip.name != Instance.audioSource.clip.name)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            audioSource.Play();
            DontDestroyOnLoad(Instance.gameObject);
        }
    }
}
