using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntities : MonoBehaviour
{
    protected StatsObject HP;
    protected StatsObject attack;
    protected StatsObject speed;

    protected enum CharacterState { Idle, Moving }
    protected CharacterState characterState { get; set; }

    void Awake()
    {
        BaseStats();
        characterState = CharacterState.Idle;
    }

    #region Stats

    private void BaseStats()
    {
        HP = new StatsObject();
        HP.Data(StatsObject.stats.HP, 10, 10);

        attack = new StatsObject();
        attack.Data(StatsObject.stats.attack, 1, 1);

        speed = new StatsObject();
        speed.Data(StatsObject.stats.speed, 5, 5);
    }

    protected void AlterateStat(StatsObject.stats statName, int? newMax, int? newCurrent)
    {
        switch(statName)
        {
            case StatsObject.stats.HP:
                HP.ChangeData(newMax, newCurrent);
                break;
            case StatsObject.stats.attack:
                attack.ChangeData(newMax, newCurrent);
                break;
            case StatsObject.stats.speed:
                speed.ChangeData(newMax, newCurrent);
                break;

        }
    }

    protected void AlterateStat(StatsObject.stats statName, int? newMax, int? newCurrent, bool canExceed)
    {
        switch (statName)
        {
            case StatsObject.stats.HP:
                HP.ChangeData(newMax, newCurrent, canExceed);
                break;
            case StatsObject.stats.attack:
                attack.ChangeData(newMax, newCurrent, canExceed);
                break;
            case StatsObject.stats.speed:
                speed.ChangeData(newMax, newCurrent, canExceed);
                break;

        }
    }

    #endregion

}
