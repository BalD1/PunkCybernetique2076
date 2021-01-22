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

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
        buttonsRef.Clear();
        GameManager.Instance.GameState = GameManager.gameState.InGame;
    }

    public void OnPointerOver(string button)
    {
        EffectsObject overAbility = new EffectsObject();
        switch (button)
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

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
    }

    public void OnPointerExit()
    {
        powerUpSummary.text = "";
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
                break;

            case GameManager.gameState.Pause:

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
