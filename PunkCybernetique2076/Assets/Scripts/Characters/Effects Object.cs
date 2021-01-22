using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsObject : ScriptableObject
{
    public enum Effect
    {
        PositiveStatModifier,
        NegativeStatModifier,
        WeaponModifier
    }
    private Effect effectType;

    private string effectName;
    public string EffectName { get => effectName; }

    private bool initialized;
    private bool temporary;

    private string summary;
    public string Summary { get => this.summary; }

    private float amount;
    public float Amount { get => this.amount; }
    private int? time;
    public int? activeTime { get => this.time; }

    private StatsObject.stats statToAffect;

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
                stat.AddPositiveModifier(stat.Max * amount);
            else if (effectType == Effect.NegativeStatModifier)
                stat.AddNegativeModifier(stat.Max * amount);
            else if (effectType == Effect.WeaponModifier)
                if (!effectName.Equals("vampire"))
                    entity.AddTickDamager(this);
                else
                    entity.Leach = this;

        }
        else if (!temporary)
        {
            if (effectType == Effect.PositiveStatModifier)
                stat.AddPositiveModifier(stat.Max * amount);
            else if (effectType == Effect.NegativeStatModifier)
                stat.AddNegativeModifier(stat.Max * amount);
            else if (effectType == Effect.WeaponModifier)
                if (effectName != "vampire")
                    entity.AddTickDamager(this);
                else
                    entity.Leach = this;
        }
    }

}
