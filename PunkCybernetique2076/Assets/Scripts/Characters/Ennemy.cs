using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : LivingEntities
{
    [SerializeField] private Transform characterTransform;

    private void Awake()
    {
        BaseStats();
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

            Debug.Log(collision);
            InflictDamage();
            Death();
        }
    }
}