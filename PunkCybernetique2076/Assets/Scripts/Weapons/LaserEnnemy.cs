using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnnemy : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private Rigidbody rb;
    public bool move;
    private GameObject linkedExplosion;
    public GameObject Glow;
    public GameObject Lightning;
    private Ennemy ennemy;
    private float damage;

    private void OnEnable()
    {
        ennemy = (Ennemy)FindObjectOfType(typeof(Ennemy));
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Unmovable Object") || collision.collider.CompareTag("Player"))
        {
            Debug.Log(collision);
            Player player = collision.collider.GetComponent<Player>();

            Debug.Log("ya collision " + collision.collider.name);

            if (player != null)
            {
                Debug.Log("c pa nul");
                player.InflictDamage(damage);
            }

            Death();
        }
    }

    private void Death()
    {
        move = false;
        this.gameObject.SetActive(false);
    }
}
