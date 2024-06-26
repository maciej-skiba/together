using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [HideInInspector] public float jumpHeight;
    [HideInInspector] public float movementSpeed;
    public static HelperStructures.Characters currentCharacter;

    private void Awake()
    {
        currentCharacter = HelperStructures.Characters.Elephant;
    }
}
