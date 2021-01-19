using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsObject : ScriptableObject
{
    public enum stats
    {
        HP,
        attack,
        speed,
        // ...
    }

    stats statName;
    int? statMax;
    int? statCurrent;
    
    public void Data(stats statName, int statMax, int statCurrent)
    {
        this.statName = statName;
        this.statMax = statMax;
        this.statCurrent = statCurrent;
    }

    public void ChangeData(int? newStatMax, int? newStatCurrent)
    {
        if (newStatMax != null)
            this.statMax = newStatMax;
        if (newStatCurrent != null)
            this.statCurrent = newStatCurrent;
    }

    public override string ToString()
    {
        string output = "";
        output += statName + " : " + statCurrent + "/" + statMax;
        return output;
    }
}
