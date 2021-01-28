using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : LivingEntities
{
    [System.Serializable]
    private struct DropableObjects
    {
        public string name;
        public int dropChances;
        public GameObject prefab;
    }

    [SerializeField] private Player player;
    [SerializeField] private float fireTimer = 1f;
    [SerializeField] private GameObject apparence;
    [SerializeField] private GameObject minimapCircle;

    [SerializeField] private AnimationCurve HPperLevel;
    [SerializeField] private AnimationCurve attackPerLevel;

    [SerializeField] private AudioSource source;

    [SerializeField] private ParticleSystem spawnEffect;

    private float nextFire;
    public int EnnemyDamage = 10;
    public float lookRadius = 30f;

    private bool dead;
    public bool Dead { get; }

    public Animator animator;
    
    private Ray ray;
    private RaycastHit hit;

    Transform target;
    NavMeshAgent agent;
    Vector3 shootpos;

    [SerializeField] private int dropChances;
    [SerializeField] private List<DropableObjects> dropableObjects;

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
        spawnEffect.Play();
        LevelToWave();
        RemoveImage("fireStatut");
        RemoveImage("poisonStatut");
        appliedTickDamagers.Clear();
        UIManager.Instance.FillBar(HP.Value / HP.Max, "HP", HPBar);
    }

    private void LevelToWave()
    {
        if (this.level.Value < GameManager.Instance.WaveNumber)
        {
            LevelUp(0, HPperLevel.Evaluate(level.Value), attackPerLevel.Evaluate(level.Value), null, null);
            LevelToWave();
        }
    }

    private void Start()
    {
        target = player.transform;
    }

    private void Update()
    {
        if (AppliedTickDamagers.Count > 0)
            if (HP.Value <= 0 && !dead)
            {
                Death();
            }

        if (!dead && GameManager.Instance.GameState == GameManager.gameState.InGame)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            animator.SetBool("IsFiring", false);
            animator.SetBool("IsIdle", true);

            if (distance <= lookRadius)
            {
                MoveToTarget(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    Physics.Linecast(this.transform.position, player.transform.position, out hit);
                    if (hit.collider.tag.Equals("Player"))
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
                            source.PlayOneShot(SoundManager.Instance.GetAudioClip(SoundManager.ClipsTags.laser));

                        }
                    }
                }
            }
        }
    }

    private void MoveToTarget(Vector3 target)
    {
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsWalking", true);
        agent.SetDestination(target);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser") && !dead)
        {
            MoveToTarget(GameManager.Instance.PlayerRef.transform.position);
            UIManager.Instance.ActivateHitMarker();
            this.InflictDamage(player.GetStatValue(StatsObject.stats.attack), player);
            if (HP.Value <= 0 && !dead)
            {
                Death();
            }
        }
    }

    private void DropObject()
    {
        int weight = 0;
        foreach(DropableObjects drop in dropableObjects)
        {
            weight += drop.dropChances;
        }

        int rand = Random.Range(0, weight + 1);
        foreach(DropableObjects drop in dropableObjects)
        {
            if (rand < weight)
            {
                Instantiate(drop.prefab, this.transform.position, Quaternion.identity);
                return;
            }
            else
                rand -= drop.dropChances;
        }

    }

    new void Death()
    {
        dead = true;
        source.PlayOneShot(SoundManager.Instance.GetAudioClip(SoundManager.ClipsTags.explosion));
        GameManager.Instance.EnnemiesLeft--;
        if (GameManager.Instance.WaveNumber == 1)
            player.GainExperience(100);
        else
            player.GainExperience(10 * (GameManager.Instance.WaveNumber / 2));
        DropObject();
        RemoveImage("fireStatut");
        RemoveImage("poisonStatut");
        appliedTickDamagers.Clear();

        this.gameObject.SetActive(false);
    }
}