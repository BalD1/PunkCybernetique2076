using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    private enum Parameters
    {
        Vignette,
    }
    [SerializeField] private PostProcessVolume volume;

    [SerializeField] private float hurtVignetteValue = 0.25f;

    private Vignette vignette;
    public Vignette Vignette { get => vignette; set => vignette = value; }
    private float vignetteFadeTimer;

    private static PostProcessManager instance;
    public static PostProcessManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("PostProcessManager instance not found.");

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        Initialization();
    }

    private void Update()
    {
        if (vignetteFadeTimer > 0)
        {
            vignetteFadeTimer -= Time.deltaTime;
            FadeTimer(vignetteFadeTimer, Parameters.Vignette);
        }
    }

    private void Initialization()
    {
        volume.profile.TryGetSettings<Vignette>(out vignette);
    }

    public void Hurt()
    {
        vignette.intensity.value = hurtVignetteValue;
        //StartCoroutine(Timer(0.2f, Parameters.Vignette));
        vignetteFadeTimer = 5;
    }

    private IEnumerator Timer(float time, Parameters parameterName)
    {

        yield return new WaitForSeconds(time);
        switch (parameterName)
        {
            case Parameters.Vignette:
                Vignette.intensity.value = 0;
                break;
        }
    }

    private void FadeTimer(float time, Parameters parameterName)
    {
        switch (parameterName)
        {
            case Parameters.Vignette:
                Vignette.intensity.value -= Time.deltaTime;
                break;
        }
    }



}
