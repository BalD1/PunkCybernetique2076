using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class Abilities
    {
        public Sprite sprite;
        public string name;
    }
    [SerializeField] private List<Abilities> abilities;
    public List<Abilities> abilitiesList { get { return abilities; } }
    private List<int> buttonsRef;

    [SerializeField] private List<Button> choices;
    [SerializeField] private GameObject powerUpCanvas;

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

    void Start()
    {

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
                foreach(Button button in choices)
                {
                    int rand = Random.Range(0, abilitiesList.Count);
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
}
