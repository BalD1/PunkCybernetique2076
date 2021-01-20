using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] private float fireTimer = 0.3f;
    [SerializeField] private GameObject laser;
    Player player;
    private float nextFire;

    private void Start()
    {
        player = transform.parent.parent.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            nextFire = Time.time + (fireTimer / player.GetStatValue(StatsObject.stats.fireRate));
            GameObject firedLaser = Instantiate(laser, this.transform.position, this.transform.parent.transform.rotation);
            firedLaser.GetComponent<Laser>().brutDamages = player.GetStatValue(StatsObject.stats.attack);
        }
    }
}
