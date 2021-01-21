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
    private Effect effectType;

    private string effectName;
    public string EffectName { get => effectName; }

    private bool initialized;
    private bool temporary;
    private bool canExceed;

    private string summary;

    private float amount;
    private int? time;

    private StatsObject.stats statToAffect;
    private StatsObject stat;
    
    private Sprite image;

    public void Data(Effect effectType, string name, float amountInPercentage, StatsObject.stats statToAffect, bool temporary, int? time, Sprite image, string summary)
    {
        if (initialized)
            throw new System.Exception("Effect already initalized.");

        this.effectType = effectType;
        this.effectName = name;
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
                stat.AddPositiveModifier((stat.Value + (stat.Value * amount)));
            else
                stat.AddNegativeModifier((stat.Value + (stat.Value * amount)));
        }
        else
        {
            if (effectType == Effect.PositiveStatModifier)
                stat.AddPositiveModifier(stat.Max + (stat.Max * amount));
            else
                stat.AddNegativeModifier(stat.Max + (stat.Max * amount));
        }
    }

    public void UnApply(LivingEntities entity)
    {
        this.amount *= -1;
        Apply(entity);
        this.amount *= -1;
    }


}
