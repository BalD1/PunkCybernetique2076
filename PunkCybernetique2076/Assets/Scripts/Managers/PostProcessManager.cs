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
    [SerializeField] private Color damagesColor = new Color(0.16f, 0, 0);

    private ColorGrading grading;
    public ColorGrading Grading { get => grading; set => grading = value; }

    private Color color;
    private float step;

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
        volume.profile.TryGetSettings<ColorGrading>(out grading);
        vignette.color.value = damagesColor;
    }

    public void Hurt()
    {
        vignette.intensity.value = hurtVignetteValue;
        InvokeRepeating("DecreaseVignette", 0.1f, 0.1f);
    }

    public void Heal()
    {

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

    public void ScreenFadeOut()
    {
        color = ChangeColor(color, Color.white, 1);
        InvokeRepeating("IncreaseVignette", 0.1f, 0.1f);
    }

    private void IncreaseVignette()
    {
        color = ChangeColor(color, Color.black, 1);     // A changer pour faire un fade
        Grading.colorFilter.value = color;
        if (color == Color.black)
        {
            CancelInvoke("IncreaseVignette");
            UIManager.Instance.GameOverScreen(true);
        }
    }

    private Color ChangeColor(Color colorToChange, Color goalColor, float step)
    {
        colorToChange = Color.Lerp(colorToChange, goalColor, step);
        return colorToChange;
    }

}
