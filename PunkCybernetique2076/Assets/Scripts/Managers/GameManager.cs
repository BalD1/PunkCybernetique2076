using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    public Player PlayerRef { get => player; }

    [SerializeField] private Ennemy ennemy;
    public Ennemy EnnemyRef { get => ennemy; set => ennemy = value; }

    private List<EffectsObject> overallEffectObjects;
    public List<EffectsObject> OverallEffectObjects { get; set; }

    public enum Rooms
    {
        HUB,
        Room1,
    }
    [SerializeField] private Transform hubSpawnPoint;
    [SerializeField] private Transform room1SpawnPoint;
    public Transform GetSpawnPoint(Rooms room)
    {
        switch (room)
        {
            case Rooms.HUB:
                return hubSpawnPoint;
            case Rooms.Room1:
                return room1SpawnPoint;
        }
        Debug.LogError("\"" + room + "\"" + " not found in switch statement.");
        return null;
    }

    private int ennemiesLeft;
    public int EnnemiesLeft
    {
        get { return ennemiesLeft; }
        set
        {
            ennemiesLeft = value;
            UIManager.Instance.UpdateEnnemiesText();
        }
    }

    private int waveNumber;
    public int WaveNumber
    {
        get { return waveNumber; }
        set
        {
            waveNumber = value;
            UIManager.Instance.UpdateWavesText();
        }
    }

    public int MaxWave { get; set; }

    public enum gameState
    {
        MainMenu,
        InGame,
        InHub,
        Pause,
        Levelup,
        Win,
        GameOver,
        Loading,
    }

    private bool isInHub;
    public bool IsInHub { get => isInHub; set => isInHub = value; }


    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("GameManager instance not found");

            return instance;
        }
    }

    #region GameState

    private gameState currentState;
    public gameState GameState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            switch (currentState)
            {
                case gameState.MainMenu:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    SceneManager.LoadScene("MainMenu");
                    break;

                case gameState.InGame:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                    if (isInHub)
                    {
                        player.transform.position = room1SpawnPoint.position;
                    }

                    isInHub = false;
                    break;

                case gameState.InHub:
                    isInHub = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                    if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
                    {
                        SceneManager.LoadScene("MainScene");
                    }
                    if (!isInHub)
                    {
                        player.transform.position = hubSpawnPoint.position;
                    }
                    break;

                case gameState.Pause:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                    break;

                case gameState.Levelup:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                    break;

                case gameState.Win:
                    SoundManager.Instance.AutomaticMusicChange();
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                    break;

                case gameState.GameOver:
                    SoundManager.Instance.AutomaticMusicChange();
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    PostProcessManager.Instance.GameOver();
                    break;
                case gameState.Loading:
                    UIManager.Instance.WindowManager(gameState.Loading);
                    StartCoroutine(LoadAsyncOperation());
                    break;

                default:
                    Debug.LogError("\"" + value + "\"" + " not found in switch statement.");
                    break;
            }
            UIManager.Instance.WindowManager(currentState);
        }
    }

    private IEnumerator LoadAsyncOperation()
    {
        yield return null;
        AsyncOperation gameLoad = SceneManager.LoadSceneAsync("MainScene");
        gameLoad.allowSceneActivation = false;
        while (!gameLoad.isDone)
        {
            UIManager.Instance.LoadLevel(gameLoad.progress);
            if (gameLoad.progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    gameLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    #endregion

    private void Awake()
    {
        instance = this;
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
            GameState = gameState.InGame;
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetAnimationLength(Animator animator, string searchedAnimation)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            if (clip.name == searchedAnimation)
                return clip.length;
        }
        Debug.LogError(" \"" + searchedAnimation + " \"" + " not found in " + animator);
        return 0;
    }


}
