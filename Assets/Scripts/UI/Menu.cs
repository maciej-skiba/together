using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Menu : MonoBehaviour
{
    private AudioClip clickSound;
    private float volume = 1.0f;
    private readonly string tempAudioName = "TempAudio";

    private void Awake()
    {
        clickSound = Resources.Load<AudioClip>("ui_click");
    }

    public void MainMenu() => SceneManager.LoadScene((int)Helpers.Scenes.Menu);
    public void Instruction() => SceneManager.LoadScene((int)Helpers.Scenes.Instruction);
    public void SelectLevel() => SceneManager.LoadScene((int)Helpers.Scenes.SelectLevel);
    public void FirstLevel() => SceneManager.LoadScene((int) Helpers.Scenes.FirstLevel);
    public void SecondLevel() => SceneManager.LoadScene((int) Helpers.Scenes.SecondLevel);
    public void ThirdLevel() => SceneManager.LoadScene((int) Helpers.Scenes.ThirdLevel);
    public void Credits() => SceneManager.LoadScene((int) Helpers.Scenes.Credits);
    
    public void PlayClickSound()
    {
        Destroy(GameObject.Find(tempAudioName));
        StartCoroutine(PlayClipAndWait());
    }

    private IEnumerator PlayClipAndWait()
    {
        GameObject tempAudioSource = new GameObject(tempAudioName);
        DontDestroyOnLoad(tempAudioSource);
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
        audioSource.volume = volume;
        audioSource.Play();

        yield return new WaitForSeconds(clickSound.length);

        Destroy(tempAudioSource);
    }
}
