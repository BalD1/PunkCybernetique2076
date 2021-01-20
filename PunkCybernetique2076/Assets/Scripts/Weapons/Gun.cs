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
    private float smokeCooldown;

    private void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            nextFire = Time.time + (fireTimer / player.GetStatValue(StatsObject.stats.fireRate));
            PoolManager.Instance.SpawnFromPool(PoolManager.tags.Laser, this.transform.position, this.transform.parent.transform.rotation);

            if (!smoke.isPlaying)
            {
                smoke.Play();
            }
            smokeCooldown = smokeTimer;
        }

        smokeCooldown = Mathf.Clamp(smokeCooldown - Time.deltaTime, 0, smokeTimer);
        if (smokeCooldown == 0)
            smoke.Stop();
    }


}
