using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    Player player;

    [SerializeField] private float fireTimer = 0.3f;
    private float nextFire;

    [SerializeField] private float smokeTimer = 3;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Animator recoil;
    [SerializeField] private LayerMask mask;
    [SerializeField] private AudioSource audio;
    private float smokeCooldown;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 center = new Vector3(0.5f, 0.5f, 0);
      

    private void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire && GameManager.Instance.GameState == GameManager.gameState.InGame)
        {
            nextFire = Time.time + (fireTimer / player.GetStatValue(StatsObject.stats.fireRate));

            ray = Camera.main.ViewportPointToRay(center);

            if (Physics.Raycast(ray, out hit, 10000))
                spawnPoint.transform.LookAt(hit.point);

            PoolManager.Instance.SpawnFromPool(PoolManager.tags.Laser, spawnPoint.transform.position, spawnPoint.transform.rotation);

            if (player.GetStatValue(StatsObject.stats.fireRate) < 2.5f && recoil != null)
            {
                recoil.speed = player.GetStatValue(StatsObject.stats.fireRate);
                recoil.SetTrigger("Fire");
            }

            if (!smoke.isPlaying)
            {
                smoke.Play();
            }
            smokeCooldown = smokeTimer;

            audio.PlayOneShot(SoundManager.Instance.GetAudioCLip("Fire"));
        }

        smokeCooldown = Mathf.Clamp(smokeCooldown - Time.deltaTime, 0, smokeTimer);
        if (smokeCooldown == 0)
            smoke.Stop();
    }
}
