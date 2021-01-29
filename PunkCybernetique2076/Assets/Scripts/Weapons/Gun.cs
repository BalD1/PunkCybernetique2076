using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Player player;

    [SerializeField] private float fireTimer = 0.3f;
    private float nextFire;

    [SerializeField] private GameObject ammoCounter;
    [SerializeField] private int maxAmmo = 20;
    [SerializeField] private float smokeTimer = 3;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem reloadSmoke;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask mask;
    [SerializeField] private AudioSource gunAudio;
    [SerializeField] private ParticleSystem fireBurst;
    private float smokeCooldown;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 center = new Vector3(0.5f, 0.5f, 0);
    private int actualAmmo;
    private bool reloading;


    private void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
        actualAmmo = maxAmmo;
        string ammoText = actualAmmo + "\n" + maxAmmo;
        UIManager.Instance.ChangeText(ammoCounter, ammoText);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && actualAmmo < maxAmmo && !reloading)
            Reload();

        if (Input.GetMouseButton(0) && !reloading)
        {
            if (Time.time > nextFire && GameManager.Instance.GameState == GameManager.gameState.InGame)
            {
                Fire();
            }

            if (actualAmmo <= 0)
                Reload();
        }
        smokeCooldown = Mathf.Clamp(smokeCooldown - Time.deltaTime, 0, smokeTimer);
        if (smokeCooldown == 0)
            smoke.Stop();
    }

    private void Fire()
    {
        nextFire = Time.time + (fireTimer / player.GetStatValue(StatsObject.stats.fireRate));

        ray = Camera.main.ViewportPointToRay(center);

        if (Physics.Raycast(ray, out hit, 10000, ~mask))
            spawnPoint.transform.LookAt(hit.point);

        PoolManager.Instance.SpawnFromPool(PoolManager.tags.Laser, spawnPoint.transform.position, spawnPoint.transform.rotation);
        actualAmmo--;
        string ammoText = actualAmmo + "\n" + maxAmmo;
        UIManager.Instance.ChangeText(ammoCounter, ammoText);

        if (player.GetStatValue(StatsObject.stats.fireRate) < 2.5f && animator != null)
        {
            animator.speed = player.GetStatValue(StatsObject.stats.fireRate);
            animator.Rebind();
            animator.SetTrigger("Fire");
        }

        if (!smoke.isPlaying)
        {
            smoke.Play();
        }
        fireBurst.Play();
        smokeCooldown = smokeTimer;

        gunAudio.PlayOneShot(SoundManager.Instance.GetAudioClip(SoundManager.ClipsTags.laser));
    }

    private void Reload()
    {
        reloadSmoke.Play();
        reloading = true;
        animator.Rebind();
        animator.SetTrigger("Reload");
        float reloadingTime = GameManager.Instance.GetAnimationLength(this.animator, "Reload");
        StartCoroutine(WaitForAnimation(reloadingTime));
    }

    private IEnumerator WaitForAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        reloadSmoke.Stop();
        actualAmmo = maxAmmo;
        string ammoText = actualAmmo + "\n" + maxAmmo;
        UIManager.Instance.ChangeText(ammoCounter, ammoText);
        reloading = false;
    }
}
