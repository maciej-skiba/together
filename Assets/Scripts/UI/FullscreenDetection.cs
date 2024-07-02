using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenDetection : MonoBehaviour
{
    [SerializeField] GameObject hint;

    private void Update()
    {
        if (Screen.fullScreen)
        {
            hint.SetActive(false);
        }
        else
        {
            hint.SetActive(true);
        }
    }
}
