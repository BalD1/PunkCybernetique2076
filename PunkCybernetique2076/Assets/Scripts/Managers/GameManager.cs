using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum gameState
    {
        MainMenu,
        InGame,
        Pause,
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
            switch (value)
            {
                case gameState.MainMenu:
                    // passer au main menu
                    break;

                case gameState.InGame:
                    // passer au in game
                    break;

                case gameState.Pause:
                    // passer en pause
                    break;

                case gameState.Win:
                    // passer en win
                    break;

                case gameState.GameOver:
                    // passer en go
                    break;
            }
        }
    }

    #endregion

    private void Awake()
    {
        instance = this;
        GameState = gameState.InGame;
    }


}
