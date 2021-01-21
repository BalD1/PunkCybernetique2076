using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntities : MonoBehaviour
{
    protected StatsObject HP;
    protected StatsObject attack;
    protected StatsObject fireRate;
    protected StatsObject speed;
    protected StatsObject level;
    protected StatsObject experience;

    private List<EffectsObject> badEffects = new List<EffectsObject>();
    private List<EffectsObject> goodEffects = new List<EffectsObject>();

    private List<StatsObject> stats = new List<StatsObject>();

    protected enum CharacterState { Idle, Moving }
    protected CharacterState characterState { get; set; }

    void Awake()
    {
        characterState = CharacterState.Idle;
        InitalizeStats();
    }

    #region Stats

    private void InitalizeStats()
    {
        stats.Add(HP);
        stats.Add(attack);
        stats.Add(fireRate);
        stats.Add(speed);
        stats.Add(level);
        stats.Add(experience);
    }

    protected void BaseStats()
    {
        level = new StatsObject();
        level.Data(StatsObject.stats.level, 100, 1);

        HP = new StatsObject();
        HP.Data(StatsObject.stats.HP, 10, 10);

        attack = new StatsObject();
        attack.Data(StatsObject.stats.attack, 1, 1);

        fireRate = new StatsObject();
        fireRate.Data(StatsObject.stats.fireRate, 1, 1);

        speed = new StatsObject();
        speed.Data(StatsObject.stats.speed, 5, 5);
    }

    public StatsObject GetStat(StatsObject.stats searchedStat)
    {
        foreach (StatsObject stat in stats)
        {
            if (stat.StatName == searchedStat)
                return stat;
        }
        Debug.Log("\"" + searchedStat + "\"" + " not found in list");
        return null;
    }

    protected void LevelUp(int newNeededExp, int? newMaxHealth, int? newMaxAttack, int? newMaxSpeed, int? newFireRate)
    {
        if (level.Value == 100)
            return;
        level.ChangeData(null, level.Value + 1);
        experience.ChangeData(newNeededExp, 0);
        if (newMaxHealth.HasValue)
            HP.ChangeData(newMaxHealth.Value, newMaxHealth.Value);
        if (newMaxAttack.HasValue)
            attack.ChangeData(newMaxAttack.Value, newMaxAttack.Value);
        if (newMaxSpeed.HasValue)
            speed.ChangeData(newMaxSpeed.Value, newMaxSpeed.Value);
        if (newFireRate.HasValue)
            fireRate.ChangeData(newFireRate.Value, newFireRate.Value);
    }

    public int GetStatValue(StatsObject.stats stat)
    {
        switch(stat)
        {
            case StatsObject.stats.attack:
                return attack.Value;
            case StatsObject.stats.experience:
                return experience.Value;
            case StatsObject.stats.fireRate:
                return fireRate.Value;
            case StatsObject.stats.HP:
                return HP.Value;
            case StatsObject.stats.level:
                return level.Value;
            case StatsObject.stats.speed:
                return speed.Value;
            default:
                throw new Exception(" \" " + stat + " \" " + "not found in switch statement.");
        }
    }

    public void ApplyEffect()
    {

    }

    public void InflictDamage(int amount)
    {
        HP.ChangeData(null, HP.Value - amount);
        if (HP.Value <= 0)
            Death();
    }

    protected void Death()
    {
        Destroy(gameObject);
    }

    #endregion

}
