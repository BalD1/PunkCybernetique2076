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
    bool initialized;

    stats statName;
    int statMax;
    int statCurrent;


    public int Value { get => this.statCurrent; }
    public int Max { get => this.statMax; }

    public void Data(stats statName, int statMax, int statCurrent)
    {
        if (initialized)
            throw new System.Exception("Stat already initalized. Please use \"Change Data\" function.");
        this.statName = statName;
        this.statMax = statMax;
        this.statCurrent = statCurrent;

        initialized = true;
    }

    public void ChangeData(int? newStatMax, int? newStatCurrent)
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
    }

    public void ChangeData(int? newStatMax, int? newStatCurrent, bool canExceed)
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
    }


    public override string ToString()
    {
        string output = statName + " : " + statCurrent + "/" + statMax;
        return output;
    }

    public void DebugLog()
    {
        Debug.Log(this.ToString());
    }
}
