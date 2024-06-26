using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class HelperFunctions
{
    public static IEnumerator CoMoveObjectToPosition(GameObject gameObject, Vector3 endPosition, float timeInSeconds)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = gameObject.transform.position;

        Debug.Log("starting position: " + startingPosition.ToString());

        while (elapsedTime < timeInSeconds)
        {
            gameObject.transform.position = Vector2.Lerp(startingPosition, endPosition, (elapsedTime / timeInSeconds));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = endPosition;
    }

    public static IEnumerator CoHideImage(Image img, float timeInSeconds)
    {
        while (img.color.a > 0.01f)
        {
            var imageColor = img.color;
            imageColor.a -= 0.02f;
            img.color = imageColor;

            yield return new WaitForSeconds(0.02f * timeInSeconds);
        }
    }

    public static IEnumerator CoHideSprite(SpriteRenderer sprite, float timeInSeconds)
    {
        while (sprite.color.a > 0.01f)
        {
            var spriteColor = sprite.color;
            spriteColor.a -= 0.02f;
            sprite.color = spriteColor;

            yield return new WaitForSeconds(0.02f * timeInSeconds);
        }
    }

    public static IEnumerator CoWaitForSecondsAndDisableObject(GameObject obj, float seconds)
    {

        yield return new WaitForSeconds(seconds);

        MonoBehaviour.Destroy(obj);
    }

    public static void DestroyAllGameObjectsOfType<T>() where T : Component
    {
        T[] gameObjects = Object.FindObjectsOfType<T>();

        foreach (T obj in gameObjects)
        {
            Object.Destroy(obj.gameObject);
        }
    }

    public static IEnumerator CoFadeOutAudioSource(AudioSource audioSource, float fadeOutTimeInSeconds)
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutTimeInSeconds)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutTimeInSeconds);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    public static IEnumerator CoFadeInAudioSource(AudioSource audioSource, float fadeOutTimeInSeconds)
    {
        audioSource.volume = 0;
        audioSource.Play();
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutTimeInSeconds)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 1, elapsedTime / fadeOutTimeInSeconds);
            yield return null;
        }

        audioSource.volume = 1f;
    }
}
