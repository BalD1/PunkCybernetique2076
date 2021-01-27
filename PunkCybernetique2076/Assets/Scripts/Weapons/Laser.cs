using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource source;

    public bool move;
    [SerializeField] private GameObject plasmaExplosion;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Death();
        }
    }

    private void Death()
    {
        move = false;
        plasmaExplosion.SetActive(true);
        plasmaExplosion.GetComponent<ParticleSystem>().Play();
        source.PlayOneShot(SoundManager.Instance.GetAudioCLip("impact"));

        StartCoroutine(SetInactive());
    }

    private IEnumerator SetInactive()
    {
        Lightning.SetActive(false);
        Glow.SetActive(false);
        this.gameObject.GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(1.8f);

        plasmaExplosion.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
