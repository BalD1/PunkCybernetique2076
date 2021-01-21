using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    private class SpriteImages
    {

    }
    [SerializeField] private GridLayoutGroup abilitiesButtonsGrid;
    [SerializeField] private List<Sprite> abilitiesImages;

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
    }


    void Start()
    {

    }



    public void OnClickEnter(string button)
    {
        switch (button)
        {
            case "First":

                break;

            case "Second":

                break;

            case "Third":

                break;

            default:
                Debug.LogError("\"" + button + "\"" + " not found in switch statement");
                break;
        }
    }
}
