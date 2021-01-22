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

    private void OnEnable()
    {
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
            Death();
        }
    }

    private void Death()
    {
        move = false;
        this.gameObject.SetActive(false);
    }
}
