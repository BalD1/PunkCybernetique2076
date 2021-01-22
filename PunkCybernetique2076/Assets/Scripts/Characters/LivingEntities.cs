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

    protected List<EffectsObject> tickDamagers;
    public List<EffectsObject> TickDamagers { get => tickDamagers; }

    protected enum CharacterState { Idle, Moving }
    protected CharacterState characterState { get; set; }

    protected void CallAwake()
    {
        characterState = CharacterState.Idle;
        BaseStats();
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
        HP.Data(StatsObject.stats.HP, 100, 100);

        attack = new StatsObject();
        attack.Data(StatsObject.stats.attack, 20, 20);

        fireRate = new StatsObject();
        fireRate.Data(StatsObject.stats.fireRate, 1, 1);

        speed = new StatsObject();
        speed.Data(StatsObject.stats.speed, 5, 5);

        tickDamagers = new List<EffectsObject>();
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

    public float GetStatValue(StatsObject.stats stat)
    {
        switch (stat)
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

    protected void LevelUp(float newNeededExp, float? newMaxHealth, float? newMaxAttack, float? newMaxSpeed, float? newFireRate)
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



    public void ApplyEffect(EffectsObject effect)
    {
        effect.Apply(this);
    }

    public void AddTickDamager(EffectsObject damager)
    {
        this.tickDamagers.Add(damager);
    }

    public void InflictDamage(float amount, LivingEntities attacker)
    {
        HP.ChangeData(null, HP.Value - amount);
        if (this.name.Equals("Player"))
            UIManager.Instance.FillBar(HP.Value / HP.Max, "HP");
        if (attacker != null)
            if (attacker.TickDamagers != null)
                foreach (EffectsObject damager in attacker.TickDamagers)
                {
                    float finalDamages = this.HP.Max - (this.HP.Max * ((100 - (float)damager.Amount) / 100));
                    finalDamages *= 100;             // Calculate how many tick damages should be dealed, based on the entity's max HP
                    InflickTickDamages(finalDamages, (int)damager.activeTime);
                }
        
        if (HP.Value <= 0)
            Death();
    }

    public void InflictDamage(float amount)
    {
        HP.ChangeData(null, HP.Value - amount);
        if (this.name.Equals("Player"))
            UIManager.Instance.FillBar(HP.Value / HP.Max, "HP");

        if (HP.Value <= 0)
            Death();
    }

    private void InflickTickDamages(float amount, int time)
    {
        InflictDamage(amount);
        StartCoroutine(TickDamage(amount, time));
    }

    private IEnumerator TickDamage(float amount, int time)
    {
        if (time <= 0)
            yield break;
        yield return new WaitForSeconds(1);
        InflickTickDamages(amount, --time);
    }

    public void LogStats(LivingEntities entity)
    {
        string output = entity.name + " stats : " + "\n" +
                        entity.level.ToString() + "\n" +
                        entity.experience.ToString() + "\n" +
                        entity.HP.ToString() + "\n" +
                        entity.attack.ToString() + "\n" +
                        entity.fireRate.ToString() + "\n" +
                        entity.speed.ToString() + "\n";
        Debug.Log(output);
    }

    protected void Death()
    {
        Destroy(gameObject);
    }

    #endregion

}
