using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class Abilities
    {
        public Sprite sprite;
        public string name;
        public enum Rarity
        {
            common,
            rare,
            legendary
        }
        public Rarity rarity;
        public int elementNumber;
    }

    [SerializeField] private List<Abilities> abilities;
    public List<Abilities> abilitiesList { get { return abilities; } set { abilities = value; } }
    [SerializeField] private int commonPercentage;
    [SerializeField] private int rarePercentage;
    [SerializeField] private int legendaryPercentage;
    private List<int> buttonsRef;

    [SerializeField] private Image loadingBarBackground;
    [SerializeField] private Image loadingBar;
    [SerializeField] private GameObject menuButtons;
    [SerializeField] private Text loadFinished;

    [SerializeField] private List<Button> choices;
    [SerializeField] private GameObject powerUpCanvas;
    [SerializeField] private TMP_Text powerUpSummary;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject HUDAndPopUpCanvas;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuAbilitiesDisplay;
    [SerializeField] private GameObject abilityDisplayImage;
    [SerializeField] private GameObject abilityDisplaySummary;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;

    [SerializeField] private Text ennemiesLeft;
    [SerializeField] private Text wave;
    [SerializeField] private Text pressSpace;

    [SerializeField] protected float ennemiesHUDimageScale = 0.1275f;

    private GameObject pauseOverAbility;
    public GameObject PauseOverAbility { get => pauseOverAbility; set => pauseOverAbility = value; }

    private float bottom;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UIManager instance not found");

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        buttonsRef = new List<int>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState.Equals(GameManager.gameState.Pause))
            if (abilityDisplaySummary.activeSelf)
            {
                PutTextOnMousePos();
            }
    }

    #region general

    public void OnClickEnter(string button)
    {
        SoundManager.Instance.Play2D(SoundManager.ClipsTags.click);
        Player player = GameManager.Instance.PlayerRef;
        switch (button)
        {
            case "First":
                ApplyEffectToPlayer(player, 0);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                break;

            case "Second":
                ApplyEffectToPlayer(player, 1);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                break;

            case "Third":
                ApplyEffectToPlayer(player, 2);
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                break;
            case "Play":
                if (GameManager.Instance.GameState.Equals(GameManager.gameState.GameOver) || 
                    GameManager.Instance.GameState.Equals(GameManager.gameState.Win))
                {
                    GameManager.Instance.ReloadScene();
                }
                if (GameManager.Instance.GameState.Equals(GameManager.gameState.MainMenu))
                    GameManager.Instance.GameState = GameManager.gameState.Loading;
                break;
            case "Continue":
                if (GameManager.Instance.IsInHub)
                    GameManager.Instance.GameState = GameManager.gameState.InHub;
                else
                    GameManager.Instance.GameState = GameManager.gameState.InGame;
                break;
            case "MainMenu":
                GameManager.Instance.GameState = GameManager.gameState.MainMenu;
                break;
            case "Quit":
                Application.Quit();
                break;

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
        buttonsRef.Clear();
    }

    public void OnPointerOver(string overObject)
    {
        EffectsObject overAbility = new EffectsObject();
        switch (overObject)
        {
            case "First":
                powerUpSummary.text = GetSummary(overAbility, 0);
                break;

            case "Second":
                powerUpSummary.text = GetSummary(overAbility, 1);
                break;

            case "Third":
                powerUpSummary.text = GetSummary(overAbility, 2);
                break;
            case "DisplayAbility":
                abilityDisplaySummary.SetActive(true);
                abilityDisplaySummary.GetComponentInChildren<TextMeshProUGUI>().text = pauseOverAbility.GetComponent<AbilityDisplayImage>().Summary;
                break;

            default:
                Debug.LogError("\"" + overObject + "\"" + " not found in switch statement");
                break;
        }
    }

    public void OnPointerExit(string overObject)
    {
        if (overObject.Equals("First") || overObject.Equals("Second") || overObject.Equals("Third"))
        {
            powerUpSummary.text = "";
            return;
        }

        switch (overObject)
        {
            case "DisplayAbility":
                abilityDisplaySummary.SetActive(false);
                break;
            default:
                Debug.LogError("\"" + overObject + "\"" + " not found in switch statement");
                break;
        }
    }

    public void WindowManager(GameManager.gameState state)
    {
        switch (state)
        {
            case GameManager.gameState.MainMenu:

                break;

            case GameManager.gameState.InGame:
                if (powerUpCanvas != null)
                    powerUpCanvas.SetActive(false);
                if (pauseMenu != null)
                    pauseMenu.SetActive(false);
                if (HUDAndPopUpCanvas != null)
                    HUDAndPopUpCanvas.SetActive(true);
                if (crosshair != null)
                    crosshair.SetActive(true);
                break;

            case GameManager.gameState.InHub:
                if (pauseMenu != null)
                    pauseMenu.SetActive(false);
                if (HUDAndPopUpCanvas != null)
                    HUDAndPopUpCanvas.SetActive(false);
                if (crosshair != null)
                    crosshair.SetActive(true);
                break;

            case GameManager.gameState.Pause:
                pauseMenu.SetActive(true);
                HUDAndPopUpCanvas.SetActive(false);
                break;

            case GameManager.gameState.Levelup:
                powerUpSummary.text = "";
                if (pressSpace.enabled == true)
                    pressSpace.enabled = false;
                crosshair.SetActive(false);
                powerUpCanvas.SetActive(true);
                foreach (Button button in choices)
                {
                    List<Abilities> tempList = GetAbilityByRarity();
                    int abilityRef = tempList[Random.Range(0, tempList.Count)].elementNumber;

                    buttonsRef.Add(abilityRef);
                    button.image.sprite = abilitiesList[abilityRef].sprite;
                }
                if (GameManager.Instance.EnnemiesLeft == 0)
                    pressSpace.enabled = true;
                break;

            case GameManager.gameState.Win:
                winScreen.SetActive(true);
                HUDAndPopUpCanvas.SetActive(false);
                powerUpCanvas.SetActive(false);
                pauseMenu.SetActive(false);
                break;

            case GameManager.gameState.GameOver:
                powerUpCanvas.SetActive(false);
                pauseMenu.SetActive(false);
                HUDAndPopUpCanvas.SetActive(false);
                break;
            case GameManager.gameState.Loading:
                loadingBarBackground.gameObject.SetActive(true);
                loadingBar.enabled = true;
                menuButtons.SetActive(false);
                break;
        }
    }

    public void LoadLevel(float amount)
    {
        loadingBar.fillAmount = amount;
        if (amount >= 0.9f)
        {
            loadFinished.gameObject.SetActive(true);
            loadingBar.fillAmount = 1;
        }

    }

    public void UpdateEnnemiesText()
    {
        ennemiesLeft.text = "Ennemies Left : " + GameManager.Instance.EnnemiesLeft.ToString();
        if (GameManager.Instance.EnnemiesLeft == 0)
            pressSpace.enabled = true;
        else
            pressSpace.enabled = false;
    }

    public void UpdateWavesText()
    {
        wave.text = "Wave " + GameManager.Instance.WaveNumber.ToString() + " / " + GameManager.Instance.MaxWave.ToString();
    }

    public void GameOverScreen(bool active)
    {
        gameOverScreen.SetActive(active);
    }

    #endregion

    #region Player Related

    private void ApplyEffectToPlayer(Player player, int buttonRef)
    {
        int effectRef = buttonsRef[buttonRef];
        GameManager.Instance.OverallEffectObjects[effectRef].Apply(player);

        GameObject sprite = Instantiate(abilityDisplayImage, pauseMenuAbilitiesDisplay.transform);
        sprite.GetComponent<Image>().sprite = abilitiesList[effectRef].sprite;
        sprite.GetComponent<AbilityDisplayImage>().Summary = GameManager.Instance.OverallEffectObjects[effectRef].Summary;

        RectTransform transform = pauseMenuAbilitiesDisplay.GetComponent<RectTransform>();
        bottom -= (sprite.GetComponent<Image>().sprite.rect.height);
        transform.offsetMin = new Vector2(transform.offsetMin.x, bottom / 2);

        Abilities appliedAbility = abilitiesList[effectRef];
        if (appliedAbility.name.Contains("_Unique"))
        {
            List<Abilities> tempList = new List<Abilities>();
            foreach (Abilities ability in abilitiesList)
            {

                if (ability.rarity == appliedAbility.rarity && !ability.name.Contains("_Unique"))
                {
                    tempList.Add(ability);
                }
            }
            if (tempList.Count > 0)
                abilitiesList[effectRef] = tempList[Random.Range(0, tempList.Count)];
        }
    }

    public void FillBar(float amount, string bar)
    {
        switch (bar)
        {
            case "HP":
                hpBar.fillAmount = amount;
                break;
            case "XP":
                xpBar.fillAmount = amount;
                break;
            default:
                Debug.LogError(bar + " bar not found in switch statement.");
                break;
        }
    }

    public void FillBar(float amount, string bar, GameObject barObject)
    {
        switch (bar)
        {
            case "HP":
                barObject.GetComponent<Image>().fillAmount = amount;
                break;
            default:
                Debug.LogError(bar + " bar not found in switch statement.");
                break;
        }
    }

    public void AddImageToEnnemyHUD(GameObject ennemyHUD, Sprite image)
    {
        GameObject child = new GameObject();
        child.transform.parent = ennemyHUD.transform;
        child.transform.localEulerAngles = Vector3.zero;
        child.transform.localPosition = Vector3.zero;
        Image addedImage = child.AddComponent<Image>();
        addedImage.sprite = image;
        addedImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ennemiesHUDimageScale);
        addedImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ennemiesHUDimageScale);
    }

    public void RemoveImageInEnnemyHUD(GameObject ennemyHUD, Sprite image)
    {
        foreach(Transform child in ennemyHUD.transform)
        {
            if (child.GetComponent<Image>().sprite == image)
            {
                Destroy(child.gameObject);
                return;
            }
        }
    }

    public void UpdateLevel(string level)
    {
        levelText.text = level;
    }

    public void ActivateHitMarker()
    {
        crosshair.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(hitmarkerTimer());
    }

    private IEnumerator hitmarkerTimer()
    {
        yield return new WaitForSeconds(0.25f);
        crosshair.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ChangeText(GameObject gO, string text)
    {
        gO.GetComponent<TextMeshProUGUI>().text = text;
        
    }

    #endregion

    #region Abilities Related

    private void PutTextOnMousePos()
    {
        Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - abilityDisplaySummary.transform.position.z));
        newPos.z = abilityDisplaySummary.transform.position.z;
        abilityDisplaySummary.transform.parent.position = Vector3.Lerp(abilityDisplaySummary.transform.position, newPos, 0.5f);
    }

    private string GetSummary(EffectsObject ability, int buttonRef)
    {
        int effectRef = buttonsRef[buttonRef];
        ability = GameManager.Instance.OverallEffectObjects[effectRef];
        return ability.Summary;
    }

    private List<Abilities> GetAbilityByRarity()
    {
        int randResult = Random.Range(1, 101);
        Abilities.Rarity searchedRarity = new Abilities.Rarity();

        List<Abilities> tempList = new List<Abilities>();

        if (randResult <= legendaryPercentage)
            searchedRarity = Abilities.Rarity.legendary;
        else if (randResult > legendaryPercentage && randResult <= (legendaryPercentage + rarePercentage))
            searchedRarity = Abilities.Rarity.rare;
        else if (randResult > (commonPercentage - 100))
            searchedRarity = Abilities.Rarity.common;

        foreach (Abilities ability in abilitiesList)
        {
            if (ability.rarity.Equals(searchedRarity))
                tempList.Add(ability);
        }

        return tempList;
    }

    #endregion




}
