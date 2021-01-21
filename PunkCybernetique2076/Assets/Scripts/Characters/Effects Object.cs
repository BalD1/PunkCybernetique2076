using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsObject : ScriptableObject
{
    public enum Effect
    {
        // Bad ones
        Poison,
        Burning,
        LowerFireRate,
        DeacreasedAttack,
        Slowed,

        // Good ones
        HealthBuff,
        AttackBuff,
        FireRateBuff,
        SpeedBuff,
        GainedExpBuff,
    }

    private bool initialized;
    private Effect effectType;
    private int amount;
    private bool temporary;
    private int? time;
    private bool canExceed;
    
    public void Data(Effect effectType, int amount, bool temporary, int? time)
    {
        if (initialized)
            throw new System.Exception("Effect already initalized.");

        this.effectType = effectType;
        this.amount = amount;
        this.temporary = temporary;
        this.time = time;
    }

    public void Apply(LivingEntities entity)
    {
        StatsObject stat;

        switch (effectType)
        {
            // Bad ones
            case Effect.Poison:
                break;

            case Effect.Burning:
                break;

            case Effect.LowerFireRate:
                stat = entity.GetStat(StatsObject.stats.fireRate);
                StatDecrease(stat);
                break;

            case Effect.DeacreasedAttack:
                stat = entity.GetStat(StatsObject.stats.attack);
                StatDecrease(stat);
                break;

            case Effect.Slowed:
                stat = entity.GetStat(StatsObject.stats.speed);
                StatDecrease(stat);
                break;


            // Good ones
            case Effect.HealthBuff:
                stat = entity.GetStat(StatsObject.stats.HP);
                StatIncrease(stat);
                break;

            case Effect.AttackBuff:
                stat = entity.GetStat(StatsObject.stats.attack);
                StatIncrease(stat);
                break;

            case Effect.FireRateBuff:
                stat = entity.GetStat(StatsObject.stats.fireRate);
                StatIncrease(stat);
                break;

            case Effect.SpeedBuff:
                stat = entity.GetStat(StatsObject.stats.speed);
                StatIncrease(stat);
                break;

            case Effect.GainedExpBuff:
                break;

            default:
                Debug.LogError("\"" + effectType + "\"" + " not found in switch statement");
                break;
        }

    }

    private void StatDecrease(StatsObject stat)
    {
        stat.ChangeData(null, stat.Value - amount);
    }

    private void StatIncrease(StatsObject stat)
    {
        stat.ChangeData(null, stat.Value + amount, canExceed);
    }

    public void UnApply(LivingEntities entity)
    {

    }

}
