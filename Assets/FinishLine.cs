using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private bool elephantAtFinishLine;
    private bool mouseAtFinishLine;

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

    private void Update()
    {
        if (IsLevelEnded())
        {
            NextLevel();
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
}
