using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("SoundManager Instance not found");

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
