using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplayImage : MonoBehaviour
{
    private bool initialized;
    private string summary;
    public string Summary
    {
        get => summary;
        set
        {
            if (!initialized)
            {
                initialized = true;
                this.summary = value;
            }
            else
                Debug.LogError(this.gameObject.name + " summary has already been set and can not be override.");
        }
    }

    public void OnPointerOver(string objectRef)
    {
        UIManager.Instance.PauseOverAbility = this.gameObject;
        UIManager.Instance.OnPointerOver(objectRef);
    }

    public void OnPointerExit(string objectRef)
    {
        UIManager.Instance.OnPointerExit(objectRef);
    }
}
