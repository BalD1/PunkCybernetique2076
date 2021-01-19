using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntities
{

    void Start()
    {
        AlterateStat(StatsObject.stats.HP, 20, 20);
        AlterateStat(StatsObject.stats.attack, 2, 2);
    }

    void Update()
    {
        // tesuto
        Debug.Log(HP.ToString());
        AlterateStat(StatsObject.stats.HP, 20, 10);
        Debug.Log(HP.ToString());

        Debug.Log(attack.ToString());
        Debug.Log(speed.ToString());
    }

    
}
