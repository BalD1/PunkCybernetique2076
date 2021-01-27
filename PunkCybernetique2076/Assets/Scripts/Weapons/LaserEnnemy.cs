using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnnemy : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource source;
    public bool move;
    private GameObject linkedExplosion;
    public GameObject Glow;
    public GameObject Lightning;
    private Ennemy ennemy;
    private float damage;

    private void OnEnable()
    {
        ennemy = GameManager.Instance.EnnemyRef;
        damage = ennemy.GetStatValue(StatsObject.stats.attack);
        this.gameObject.GetComponent<Renderer>().enabled = true;
        Lightning.SetActive(true);
        Glow.SetActive(true);
        move = true;
    }

    private void FixedUpdate()
    {
        if (move)
            rb.MovePosition(this.transform.position + (transform.forward * (speed * Time.deltaTime)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unmovable Object") || other.CompareTag("Player") || other.CompareTag("Laser"))
        {
            Player player = other.GetComponentInChildren<Player>();

            if (player != null)
            {
                player.InflictDamage(damage);
            }
            source.PlayOneShot(SoundManager.Instance.GetAudioCLip("impact"));

            Death();
        }
    }

    private void Death()
    {
        move = false;
        this.gameObject.SetActive(false);
    }
}
