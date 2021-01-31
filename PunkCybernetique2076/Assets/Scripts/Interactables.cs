using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    [SerializeField] private Transform checkSphere;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask checkMask;
    private bool isInRange;

    [SerializeField] private GameObject clickToEnter;
    [SerializeField] private GameObject mainScreen;

    private void Update()
    {
        isInRange = Physics.CheckSphere(checkSphere.position, checkRadius, checkMask);
    }

    private void OnMouseOver()
    {
        if (isInRange)
            if (!GameManager.Instance.IsInteracting)
                GameManager.Instance.IsInteracting = true;

        if (!isInRange)
            if (GameManager.Instance.IsInteracting)
                GameManager.Instance.IsInteracting = false;
    }

    private void OnMouseExit()
    {
        GameManager.Instance.IsInteracting = false;
    }

    public void Click(string state)
    {
        switch (state)
        {
            case "enter":
                clickToEnter.SetActive(false);
                mainScreen.SetActive(true);
                break;
        }
    }

    public void RoomChoice(int number)
    {
        List<GameManager.BattleRooms> roomsList = GameManager.Instance.BattleRoomsList;
        foreach(GameManager.BattleRooms room in roomsList)
        {
            if (room.number == number)
            {
                Instantiate(room.map);
            }
        }
        GameManager.Instance.GameState = GameManager.gameState.InGame;
    }

    

}
