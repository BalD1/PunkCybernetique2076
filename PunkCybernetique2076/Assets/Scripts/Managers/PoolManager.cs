using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public tags tag;
        public GameObject prefab;
        public int size;
    }

    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("PoolManager Instance not found");

                return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public enum tags
    {
        Laser,
        PlasmaExplosion
    }

    public List<Pool> pools;
    public Dictionary<tags, Queue<GameObject>> poolDictionnary;

    private void Start()
    {
        poolDictionnary = new Dictionary<tags, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionnary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (tags tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionnary.ContainsKey(tag))
        {
            Debug.LogError("Pool with tag " + tag + " doesn't exist");
            return null;
        }
        GameObject objToSpawn = poolDictionnary[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        poolDictionnary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }


}
