using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsObject : ScriptableObject
{
    public enum Effect
    {
        PositiveStatModifier,
        NegativeStatModifier,
    }

    private bool initialized;
    private Effect effectType;
    private int amount;
    private StatsObject.stats statToAffect;
    private bool temporary;
    private int? time;
    private Sprite image;
    private string summary;
    private bool canExceed;
    private StatsObject stat;

    public void Data(Effect effectType, int amountInPercentage, StatsObject.stats statToAffect, bool temporary, int? time, Sprite image, string summary)
    {
        if (initialized)
            throw new System.Exception("Effect already initalized.");

        this.effectType = effectType;
        this.amount = amountInPercentage / 100;
        this.statToAffect = statToAffect;
        this.temporary = temporary;
        this.time = time;
        this.image = image;
        this.summary = summary;
    }

    public void Apply(LivingEntities entity)
    {
        StatsObject stat = entity.GetStat(statToAffect);
        if (temporary)
        {
            if (effectType == Effect.PositiveStatModifier)
                stat.ChangeData(null, stat.Value + (stat.Value * amount), true);
            else
                stat.ChangeData(null, stat.Value - (stat.Value * amount));
        }
        else
        {
            if (effectType == Effect.PositiveStatModifier)
                stat.ChangeData(stat.Max + (stat.Max * amount), stat.Value + (stat.Value * amount));
            else
                stat.ChangeData(stat.Max - (stat.Max * amount), stat.Value - (stat.Value * amount));
        }
    }

    public void UnApply(LivingEntities entity)
    {
        this.amount *= -1;
        Apply(entity);
        this.amount *= -1;
    }


}
