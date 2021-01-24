using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource source;

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
        if (!collision.collider.CompareTag("Player"))
        {
            Death();
        }
    }

    private void Death()
    {
        move = false;
        if (linkedExplosion == null)
        {
            linkedExplosion = PoolManager.Instance.SpawnFromPool(PoolManager.tags.PlasmaExplosion, this.transform.position, Quaternion.identity);
            linkedExplosion.transform.position = this.transform.position;
            /* j'arrive pas à régler ce bug. Les explosions vont rester au premier
             * endroit où elles ont été activées, peu importe la position que
             * j'essaie de leur donner.
             * J'ai tout essayé, j'ai changé leur simulation space, essayé de les mettre
             * dans un gameobject vide, j'ai fais de la magie vaudou...
             * Je suis vraiment très curieux du soucis pour le coup, j'ai pas réussi à le
             * comprendre
             */
        }
        source.PlayOneShot(SoundManager.Instance.GetAudioCLip("impact"));
        linkedExplosion.SetActive(true);
        StartCoroutine(SetInactive());
    }

    private IEnumerator SetInactive()
    {
        Lightning.SetActive(false);
        Glow.SetActive(false);
        this.gameObject.GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(1.8f);
        
        linkedExplosion.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
