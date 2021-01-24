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
    [SerializeField] private GameObject minimapCircle;

    [SerializeField] private AnimationCurve HPperLevel;
    [SerializeField] private AnimationCurve attackPerLevel;

    [SerializeField] private AudioSource source;

    private float nextFire;
    public int EnnemyDamage = 10;
    public float lookRadius = 30f;

    private bool dead;
    public bool Dead { get; }

    public Animator animator;

    Transform target;
    NavMeshAgent agent;
    Vector3 shootpos;

    private void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance.PlayerRef;
        CallAwake();
        GameManager.Instance.EnnemyRef = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.EnnemyRef = this;
        this.apparence.SetActive(true);
        dead = false;
        minimapCircle.SetActive(true);
        LevelToWave();
    }

    private void LevelToWave()
    {
        if (this.level.Value < GameManager.Instance.WaveNumber)
        {
            LevelUp(0, HPperLevel.Evaluate(level.Value), attackPerLevel.Evaluate(level.Value), null, null);
            Debug.Log(HPperLevel.Evaluate(level.Value));
            LevelToWave();
        }
    }

    private void Start()
    {
        target = player.transform;
    }

    private void Update()
    {
        if (!dead && GameManager.Instance.GameState == GameManager.gameState.InGame)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            animator.SetBool("IsFiring", false);
            animator.SetBool("IsIdle", true);

            if (distance <= lookRadius)
            {
                animator.SetBool("IsIdle", false);
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsFiring", true);
                    FaceTarget();

                    //Attack 

                    if (Time.time > nextFire)
                    {
                        shootpos = new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, this.transform.position.z);

                        nextFire = Time.time + (fireTimer / 0.5f);
                        PoolManager.Instance.SpawnFromPool(PoolManager.tags.LaserEnnemy, shootpos, this.transform.rotation);
                        source.PlayOneShot(SoundManager.Instance.GetAudioCLip("laser"));

                    }
                }
                else
                    animator.SetBool("IsWalking", true);
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
        if (collision.collider.CompareTag("Laser") && !dead)
        {
            UIManager.Instance.ActivateHitMarker();
            this.InflictDamage(player.GetStatValue(StatsObject.stats.attack), player);
            if (HP.Value <= 0 && !dead)
            {
                Death();
            }
        }
    }

    new void Death()
    {
        dead = true;
        SoundManager.Instance.Play("boom");
        GameManager.Instance.EnnemiesLeft--;
        if (GameManager.Instance.WaveNumber == 1)
            player.GainExperience(100);
        else
            player.GainExperience(10 * (GameManager.Instance.WaveNumber / 2));
        this.gameObject.SetActive(false);
    }
}