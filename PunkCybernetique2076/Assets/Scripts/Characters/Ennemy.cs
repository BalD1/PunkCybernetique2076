using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : LivingEntities
{
    [SerializeField] private Player player;
    [SerializeField] private Transform characterTransform;

    private void Awake()
    {
        CallAwake();
    }

    private void Start()
    {
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Laser"))
        {

            Debug.Log(this.HP);

            InflictDamage(player.GetStatValue(StatsObject.stats.attack));
            Death();
        }
    }

    new void Death()
    {
        SoundManager.Instance.Play("boom");
        Destroy(gameObject);
    }
}