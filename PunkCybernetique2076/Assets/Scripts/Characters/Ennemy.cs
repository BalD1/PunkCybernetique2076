using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : LivingEntities
    {
    #region Variables Declaration

    [SerializeField] private Player player;

    [SerializeField] private LayerMask mask;
    private Ray ray;
    private RaycastHit hit;
    
    #region Data

    [SerializeField] private float fireTimer = 1f;

    [SerializeField] private GameObject apparence;
    [SerializeField] private GameObject minimapCircle;
    [SerializeField] private ParticleSystem spawnEffect;
    public Animator animator;
    Transform target;
    NavMeshAgent agent;
    [SerializeField] private GameObject shootpos;

    [SerializeField] private AnimationCurve HPperLevel;
    [SerializeField] private AnimationCurve attackPerLevel;
    [SerializeField] private AnimationCurve givenExperiencePerlevel;

    private float nextFire;
    public float lookRadius = 30f;

    private bool dead;
    public bool Dead { get; }

    [SerializeField] private AudioSource source;

    #endregion

    #region Items drop

    [System.Serializable]
    private struct DropableObjects
    {
        public string name;
        public int dropChances;
        public GameObject prefab;
    }

    [SerializeField] private int dropChances;
    [SerializeField] private List<DropableObjects> dropableObjects;

    #endregion

    #endregion

    #region Functions

    #region Awake, Start, Update

    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance.PlayerRef;
        CallAwake();
        GameManager.Instance.EnnemyRef = this;
    }

    private void Start()
    {
        target = player.transform;
    }

    private void OnEnable()
    {
        GameManager.Instance.EnnemyRef = this;
        this.apparence.SetActive(true);
        dead = false;
        minimapCircle.SetActive(true);
        spawnEffect.Play();
        RemoveImage("fireStatut");
        RemoveImage("poisonStatut");
        appliedTickDamagers.Clear();
        this.HP.ChangeData(null,this.HP.Max);
        UIManager.Instance.FillBar(HP.Value / HP.Max, "HP", HPBar);
    }

    public void LevelToWave(int lvl)
    {
        Setlevel(lvl, 0, HPperLevel.Evaluate(level.Value), attackPerLevel.Evaluate(level.Value), null, null);
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
                    Physics.Linecast(this.transform.position, player.transform.position, out hit, ~mask);
                    if (hit.collider.tag.Equals("Player"))
                    {
                        animator.SetBool("IsWalking", false);
                        animator.SetBool("IsFiring", true);
                        FaceTarget();

                        //Attack 

                        if (Time.time > nextFire)
                        {
                            shootpos.transform.LookAt(player.transform.position);
                            nextFire = Time.time + (fireTimer / 0.5f);
                            PoolManager.Instance.SpawnFromPool(PoolManager.tags.LaserEnnemy, shootpos.transform.position, shootpos.transform.rotation);
                            source.PlayOneShot(SoundManager.Instance.GetAudioClip(SoundManager.ClipsTags.laser));

                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Mechanics

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
        foreach (DropableObjects drop in dropableObjects)
        {
            weight += drop.dropChances;
        }

        int rand = Random.Range(0, weight + 1);
        foreach (DropableObjects drop in dropableObjects)
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
            player.GainExperience(givenExperiencePerlevel.Evaluate(this.level.Value));

        if (Random.Range(0, 101) < dropChances)
            DropObject();
        RemoveImage("fireStatut");
        RemoveImage("poisonStatut");
        appliedTickDamagers.Clear();

        this.gameObject.SetActive(false);
    }

    #endregion

    #region Misc

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    #endregion
    
    #endregion
}