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


    [SerializeField] private List<Button> choices;
    [SerializeField] private GameObject powerUpCanvas;
    [SerializeField] private TMP_Text powerUpSummary;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private GameObject HUDAndPopUpCanvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuAbilitiesDisplay;
    [SerializeField] private GameObject abilityDisplayImage;
    [SerializeField] private GameObject abilityDisplaySummary;
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
        if (abilityDisplaySummary.activeSelf)
        {
            PutTextOnMousePos();
        }
    }

    private void PutTextOnMousePos()
    {
        Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - abilityDisplaySummary.transform.position.z));
        newPos.z = abilityDisplaySummary.transform.position.z;
        abilityDisplaySummary.transform.parent.position = Vector3.Lerp(abilityDisplaySummary.transform.position, newPos, 0.5f);
    }

    public void OnClickEnter(string button)
    {
        Player player = GameManager.Instance.PlayerRef;
        switch (button)
        {
            case "First":
                ApplyEffectToPlayer(player, 0);
                break;

            case "Second":
                ApplyEffectToPlayer(player, 1);
                break;

            case "Third":
                ApplyEffectToPlayer(player, 2);
                break;
            case "Play":
                GameManager.Instance.GameState = GameManager.gameState.InGame;
                break;
            case "Continue":
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
        GameManager.Instance.GameState = GameManager.gameState.InGame;
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

    private string GetSummary(EffectsObject ability, int buttonRef)
    {
        int effectRef = buttonsRef[buttonRef];
        ability = GameManager.Instance.OverallEffectObjects[effectRef];
        return ability.Summary;
    }

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


    public void WindowManager(GameManager.gameState state)
    {
        switch (state)
        {
            case GameManager.gameState.MainMenu:

                break;

            case GameManager.gameState.InGame:
                powerUpCanvas.SetActive(false);
                pauseMenu.SetActive(false);
                HUDAndPopUpCanvas.SetActive(true);
                break;

            case GameManager.gameState.Pause:
                pauseMenu.SetActive(true);
                HUDAndPopUpCanvas.SetActive(false);
                break;

            case GameManager.gameState.Levelup:
                powerUpCanvas.SetActive(true);
                foreach (Button button in choices)
                {
                    List<Abilities> tempList = GetAbilityByRarity();
                    int abilityRef = tempList[Random.Range(0, tempList.Count)].elementNumber;

                    buttonsRef.Add(abilityRef);
                    button.image.sprite = abilitiesList[abilityRef].sprite;
                }
                break;

            case GameManager.gameState.Win:

                break;

            case GameManager.gameState.GameOver:

                break;
        }
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

    public void UpdateLevel(string level)
    {
        levelText.text = level;
    }
}
