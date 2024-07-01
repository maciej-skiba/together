using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _musicOnOffText;
    private bool _musicOn = true;
    private AudioClip _clickSound;

    private void Awake()
    {
        _clickSound = Resources.Load<AudioClip>("ui_click");
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
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));
    }

    public void SwitchMusicOnOff()
    {
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));

        switch (_musicOn)
        {
            case true:
                _musicOn = false;
                _musicOnOffText.text = "Music: Off";
                break;
            case false:
                _musicOn = true;
                _musicOnOffText.text = "Music: On";
                break;
        }
    }
}
