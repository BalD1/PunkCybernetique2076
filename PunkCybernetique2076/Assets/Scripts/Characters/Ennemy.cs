using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : LivingEntities
{
    [SerializeField] private Player player;
    [SerializeField] private float fireTimer = 1f;
    [SerializeField] private GameObject apparence;
    private float nextFire;
    public int EnnemyDamage = 10;
    public float lookRadius = 30f;

    private bool dead;

    private GameObject linkedExplosion;

    Transform target;
    NavMeshAgent agent;
    Vector3 shootpos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        CallAwake();
    }

    private void Start()
    {
        target = player.transform;
    }

    private void Update()
    {
        if (!dead)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();

                    //Attack 

                    if (Time.time > nextFire)
                    {
                        shootpos = new Vector3(this.transform.position.x, this.transform.position.y + 0.4f, this.transform.position.z);

                        nextFire = Time.time + (fireTimer / 0.5f);
                        PoolManager.Instance.SpawnFromPool(PoolManager.tags.LaserEnnemy, shootpos, this.transform.rotation);
                        SoundManager.Instance.Play("laser");

                    }
                }
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Laser"))
        {
            UIManager.Instance.ActivateHitMarker();
            this.InflictDamage(player.GetStatValue(StatsObject.stats.attack), player);
            if (HP.Value <= 0)
            {
                Death();
            }
        }
    }

    new void Death()
    {
        dead = true;
        SoundManager.Instance.Play("boom");

        if (linkedExplosion == null)
            linkedExplosion = PoolManager.Instance.SpawnFromPool(PoolManager.tags.NormalExplosion, this.transform.position, Quaternion.identity);
        linkedExplosion.SetActive(true);
        StartCoroutine(SetInactive());
    }

    private IEnumerator SetInactive()
    {
        this.apparence.SetActive(false);

        yield return new WaitForSeconds(1.8f);
        linkedExplosion.SetActive(false);
        Destroy(this.gameObject);
    }
}