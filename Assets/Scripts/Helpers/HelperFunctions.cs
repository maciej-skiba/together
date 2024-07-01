using System;
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

    public static IEnumerator CoShowImage(Image img, float timeInSeconds)
    {
        while (img.color.a < 1.0f)
        {
            var imageColor = img.color;
            imageColor.a += 0.01f;
            img.color = imageColor;

            yield return new WaitForSeconds(0.02f * timeInSeconds);
        }
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

    public static IEnumerator CoChangeImageAlpha(Image img, float timeInSeconds, float alpha)
    {
        if (img.color.a > alpha)
        {
            while (img.color.a > alpha)
            {
                var imageColor = img.color;
                imageColor.a -= 0.01f;
                img.color = imageColor;

                yield return new WaitForSeconds(0.01f * timeInSeconds);
            }
        }
        else
        {
            while (img.color.a < alpha)
            {
                var imageColor = img.color;
                imageColor.a += 0.01f;
                img.color = imageColor;

                yield return new WaitForSeconds(0.01f * timeInSeconds);
            }
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
        T[] gameObjects = UnityEngine.Object.FindObjectsOfType<T>();

        foreach (T obj in gameObjects)
        {
            UnityEngine.Object.Destroy(obj.gameObject);
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

    public static IEnumerator CoEnableCollider2DAfterSeconds(Collider2D collider, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        collider.enabled = true;
    }

    public static IEnumerator CoDisableCollider2DAfterSeconds(Collider2D collider, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        collider.enabled = false;
    }

    public static IEnumerator CoChangePositionAfterSeconds(Transform transform, Vector3 targetPosition, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        transform.position = targetPosition;
    }

    public static IEnumerator CoRunMethodWithParamAfterSeconds<T>(Action<T> method, T parameter, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        method(parameter);
    }

    public static bool IsLayerInRange(Vector2 direction, string targetLayerName, float distance, GameObject sourceObject)
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer(targetLayerName);
        RaycastHit2D hit = Physics2D.Raycast(sourceObject.transform.position, direction, distance, layerMask);

        Debug.DrawRay(sourceObject.transform.position, direction * hit.distance, Color.red);        

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsObjectInRange(Vector2 source, Vector2 target, float range)
    {
        if (Vector2.Distance(source, target) <= range) return true;

        return false;
    }

    public static bool AnimatorIsPlaying(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static void ChangePosition(Transform transform, Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
}
