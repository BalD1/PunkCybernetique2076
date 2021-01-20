using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private Rigidbody rb;
    public bool move;
    private GameObject linkedExplosion;

    private void OnEnable()
    {
        move = true;
    }

    private void FixedUpdate()
    {
        if (move)
            rb.MovePosition(this. transform.position + (transform.forward * (speed * Time.deltaTime)));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Unmovable Object"))
        {
            Debug.Log(collision);
            Death();
        }
    }

    private void Death()
    {
        move = false;
        linkedExplosion = PoolManager.Instance.SpawnFromPool(PoolManager.tags.PlasmaExplosion, this.transform.position, Quaternion.identity);
        StartCoroutine(SetInactive());
    }

    private IEnumerator SetInactive()
    {

        yield return new WaitForSeconds(1.8f);

        linkedExplosion.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
