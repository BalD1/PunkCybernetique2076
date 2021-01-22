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
    }
    [SerializeField] private List<Abilities> abilities;
    public List<Abilities> abilitiesList { get { return abilities; } }
    [SerializeField] private int commonPercentage;
    [SerializeField] private int rarePercentage;
    [SerializeField] private int legendaryPercentage;
    private List<int> buttonsRef;


    [SerializeField] private List<Button> choices;
    [SerializeField] private GameObject powerUpCanvas;
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
                GameManager.Instance.OverallEffectObjects[buttonsRef[0]].Apply(player);
                break;

            case "Second":
                GameManager.Instance.OverallEffectObjects[buttonsRef[1]].Apply(player);
                break;

            case "Third":
                GameManager.Instance.OverallEffectObjects[buttonsRef[2]].Apply(player);
                break;

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
        buttonsRef.Clear();
        GameManager.Instance.GameState = GameManager.gameState.InGame;
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
                    int rand = Random.Range(0, tempList.Count);
                    buttonsRef.Add(rand);
                    button.image.sprite = abilitiesList[rand].sprite;
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
        if (randResult < 60)
            searchedRarity = Abilities.Rarity.common;

        List<Abilities> tempList = new List<Abilities>();

        foreach (Abilities ability in abilitiesList)
            if (ability.rarity == Abilities.Rarity.common)
                tempList.Add(ability);
        
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
