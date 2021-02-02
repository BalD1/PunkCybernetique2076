using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private AnimationCurve ennemiesToSpawnByWave;
    [SerializeField] private AnimationCurve ennemiesLevelByWave;
    private int ennemiesLeft;
    private List<GameObject> spawners;
    private GameObject spawnedEnnemy;

    private void Awake()
    {
        spawners = new List<GameObject>();
        GameManager.Instance.MaxWave = (int)this.ennemiesToSpawnByWave[ennemiesToSpawnByWave.length - 1].time;
        GameManager.Instance.WaveNumber = 0;
        GetChildsInList();
        ennemiesLeft = 0;
        GameManager.Instance.EnnemiesLeft = ennemiesLeft;
    }

    private void Update()
    {
        ennemiesLeft = GameManager.Instance.EnnemiesLeft;
        NextState();
    }

    private void NextState()
    {
        if (GameManager.Instance.WaveNumber == GameManager.Instance.MaxWave && ennemiesLeft == 0 && Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Instance.UnlockNextMap();
            GameManager.Instance.GameState = GameManager.gameState.InHub;
        }

        else if (ennemiesLeft == 0 && Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.GameState.Equals(GameManager.gameState.InGame))
            SpawnEnnemies();
    }

    private void SpawnEnnemies()
    {
        if (ennemiesLeft < ennemiesToSpawnByWave[GameManager.Instance.WaveNumber].value)
        {
            int index = Random.Range(0, spawners.Count);
            Vector3 pos = spawners[index].transform.position;
            spawnedEnnemy = PoolManager.Instance.SpawnFromPool(PoolManager.tags.Ennemy, pos, Quaternion.identity);
            spawnedEnnemy.GetComponent<Ennemy>().LevelToWave((int)ennemiesLevelByWave[GameManager.Instance.WaveNumber].value);
            GameManager.Instance.EnnemiesLeft++;
            ennemiesLeft++;
        }
        else
        {
            GameManager.Instance.WaveNumber++;
            return;
        }

        spawnedEnnemy = null;
        SpawnEnnemies();
    }

    private void GetChildsInList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawners.Add(transform.GetChild(i).gameObject);
        }
    }

}
