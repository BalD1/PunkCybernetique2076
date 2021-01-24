using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    private enum Parameters
    {
        Vignette,
        Chromatic,
    }
    [SerializeField] private PostProcessVolume volume;

    [SerializeField] private float hurtVignetteValue = 0.25f;

    private ChromaticAberration chromatic;
    public ChromaticAberration Chromatic { get => chromatic; set => chromatic = value; }

    private Vignette vignette;
    public Vignette Vignette { get => vignette; set => vignette = value; }

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
    }

    private void Initialization()
    {
        volume.profile.TryGetSettings<Vignette>(out vignette);
        volume.profile.TryGetSettings<ChromaticAberration>(out chromatic);
    }

    public void Hurt()
    {
        vignette.intensity.value = hurtVignetteValue;
        InvokeRepeating("DecreaseVignette", 0.1f, 0.1f);
    }

    private void DecreaseVignette()
    {
        vignette.intensity.value -= Time.deltaTime;
        if (vignette.intensity.value <= 0)
            CancelInvoke("DecreaseVignette");
    }

    public void GameOver()
    {
        InvokeRepeating("IncreaseChromatic", 0.1f, 0.1f);
    }

    private void IncreaseChromatic()
    {
        chromatic.intensity.value += Time.deltaTime * 5;
        if (chromatic.intensity.value >= 3)
            CancelInvoke("IncreaseChromatic");
    }

}
