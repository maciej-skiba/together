using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private bool elephantAtFinishLine;
    private bool mouseAtFinishLine;
    private bool levelEndSoundPlaying;
    private AudioSource levelEndSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Helpers.ElephantLayerName))
        {
            elephantAtFinishLine = true;
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer(Helpers.MouseLayerName))
        {
            mouseAtFinishLine = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(Helpers.ElephantLayerName))
        {
            elephantAtFinishLine = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer(Helpers.MouseLayerName))
        {
            mouseAtFinishLine = false;
        }
    }

    private void Awake()
    {
        levelEndSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsLevelEnded() && !levelEndSoundPlaying)
        {
            StartCoroutine(LevelEnd());
        }
    }

    private bool IsLevelEnded()
    {
        return elephantAtFinishLine && mouseAtFinishLine;
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LevelEnd()
    {
        levelEndSound.Play();
        levelEndSoundPlaying = true;

        while (levelEndSound.isPlaying)
        {
            yield return null;
        }

        NextLevel();
    }
}
