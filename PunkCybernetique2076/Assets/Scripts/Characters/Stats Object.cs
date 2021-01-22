using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsObject : ScriptableObject
{
    public enum stats
    {
        HP,
        attack,
        fireRate,
        speed,
        level,
        experience,
        // ...
    }
    private bool initialized;

    private stats statName;
    private float statMax;
    private float statCurrent;
    private float modifier;

    public stats StatName { get => this.statName; }
    public float Value { get => this.statCurrent; }
    public float Max { get => this.statMax; }
    public float Modifier { get => this.modifier; }
    
    public void Data(stats statName, float statMax, float statCurrent)
    {
        if (initialized)
            throw new System.Exception("Stat already initalized. Please use \"Change Data\" function.");
        this.statName = statName;
        this.statMax = statMax;
        this.statCurrent = statCurrent;

        initialized = true;
    }

    public void ChangeData(float? newStatMax, float? newStatCurrent)
    {
        if (newStatMax.HasValue)
            this.statMax = newStatMax.Value;
        if (newStatCurrent.HasValue)
        {
            if (newStatCurrent >= statMax)
                this.statCurrent = this.statMax;
            else
                this.statCurrent = newStatCurrent.Value;
        }

        this.statCurrent += modifier;
    }

    public void ChangeData(float? newStatMax, float? newStatCurrent, bool canExceed)
    {
        if (newStatMax.HasValue)
            this.statMax = newStatMax.Value;
        if (newStatCurrent != null)
        {
            if (newStatCurrent > statMax && canExceed)
                this.statCurrent = newStatCurrent.Value;
            else if (newStatCurrent > statMax && !canExceed)
                this.statCurrent = this.statMax;
            else if (newStatCurrent <= statMax)
                this.statCurrent = newStatCurrent.Value;
        }
        if (this.statCurrent < 0)
            statCurrent = 0;
        if (this.statMax < 0)
            statMax = 0;

        this.statCurrent += modifier;
    }

    public void AddPositiveModifier(float modifier)
    {
        this.modifier += modifier;
        this.ChangeData(null, null);
    }

    public void AddNegativeModifier(float modifier)
    {
        this.modifier -= modifier;
    }

    public override string ToString()
    {
        string output = statName + " : " + statCurrent + " ( + " + modifier + " ) / " + statMax;
        return output;
    }

    public void DebugLog()
    {
        Debug.Log(this.ToString());
    }
}
