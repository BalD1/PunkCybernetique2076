﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    public Player PlayerRef { get => player; }

    private List<EffectsObject> overallEffectObjects;
    public List<EffectsObject> OverallEffectObjects { get; set; }

    public enum gameState
    {
        MainMenu,
        InGame,
        Pause,
        Levelup,
        Win,
        GameOver
    }


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
                    // passer au main menu
                    break;

                case gameState.InGame:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                    break;

                case gameState.Pause:
                    // passer en pause
                    break;

                case gameState.Levelup:
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                    break;

                case gameState.Win:
                    // passer en win
                    break;

                case gameState.GameOver:
                    // passer en go
                    break;

                default:
                    Debug.LogError("\"" + value + "\"" + " not found in switch statement.");
                    break;
            }
            UIManager.Instance.WindowManager(currentState);
        }
    }

    #endregion

    private void Awake()
    {
        instance = this;
        GameState = gameState.InGame;
    }

}
