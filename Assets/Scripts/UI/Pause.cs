using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    static public bool gamePaused = false;

    public static Pause Instance { get; private set; }

    private GameObject pauseWindow;

    private Pause() { }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        pauseWindow = this.gameObject;
    }

    public void PauseOn()
    {
        Time.timeScale = 0;
        pauseWindow.SetActive(true);
        gamePaused = true;
    }

    public void PauseOff()
    {
        Time.timeScale = 1;
        pauseWindow.SetActive(false);
        gamePaused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene((int)Helpers.Scenes.Menu);
    }
}
