using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{
    [SerializeField] private Transform checkSphere;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask checkMask;
    private bool isInRange;

    [SerializeField] private GameObject clickToEnter;
    [SerializeField] private GameObject mainScreen;

    [SerializeField] private List<GameObject> lockableObjects;

    [SerializeField] private Sprite unlockedImage;
    [SerializeField] private Sprite lockedImage;

    private void Update()
    {
        isInRange = Physics.CheckSphere(checkSphere.position, checkRadius, checkMask);
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.GameState.Equals(GameManager.gameState.Pause))
            return;

        if (isInRange)
            if (!GameManager.Instance.IsInteracting)
                GameManager.Instance.IsInteracting = true;

        if (!isInRange)
            if (GameManager.Instance.IsInteracting)
                GameManager.Instance.IsInteracting = false;
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.GameState.Equals(GameManager.gameState.Pause))
            return;

        GameManager.Instance.IsInteracting = false;
    }

    public void Click(string state)
    {
        switch (state)
        {
            case "enter":
                int unlockedRooms = GameManager.Instance.UnlockedRooms;
                for (int i = 0; i <= unlockedRooms - 1; i++)
                    lockableObjects[i].GetComponent<Image>().sprite = unlockedImage;

                clickToEnter.SetActive(false);
                mainScreen.SetActive(true);
                break;
        }
    }

    public void RoomChoice(int number)
    {
        if (lockableObjects[(number - 1)].GetComponent<Image>().sprite.Equals(lockedImage))
            return;

        List<GameManager.BattleRooms> roomsList = GameManager.Instance.BattleRoomsList;
        if (GameManager.Instance.GameState.Equals(GameManager.gameState.InHub))
            foreach (GameManager.BattleRooms room in roomsList)
            {
                if (room.number == number)
                {
                    GameObject map = Instantiate(room.map);
                    GameManager.Instance.instantiatedMap = map;
                    GameManager.Instance.instantiatedMapRef = room.number;
                }
            }
        if (GameManager.Instance.instantiatedMap == null)
        {
            Debug.LogError("Map number \"" + number + "\"" + " not found.");
            return;
        }
        clickToEnter.SetActive(true);
        mainScreen.SetActive(false);
        GameManager.Instance.GameState = GameManager.gameState.InGame;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkSphere.position, checkRadius);
    }

}
