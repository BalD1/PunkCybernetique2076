﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private AnimationCurve ennemiesToSpawnByWave;
    private int ennemiesLeft;
    private List<GameObject> spawners;

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
        if (ennemiesLeft == 0 && GameManager.Instance.WaveNumber == GameManager.Instance.MaxWave)
            GameManager.Instance.GameState = GameManager.gameState.Win;
        ennemiesLeft = GameManager.Instance.EnnemiesLeft;
        if (ennemiesLeft == 0 && Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.GameState == GameManager.gameState.InGame)
            SpawnEnnemies();
    }

    private void SpawnEnnemies()
    {
        if (ennemiesLeft < ennemiesToSpawnByWave[GameManager.Instance.WaveNumber].value)
        {
            int index = Random.Range(0, spawners.Count);
            Vector3 pos = spawners[index].transform.position;
            pos.x += Random.Range(-0.5f, 0.5f);
            pos.z += Random.Range(-0.5f, 0.5f);
            PoolManager.Instance.SpawnFromPool(PoolManager.tags.Ennemy, pos, Quaternion.identity);
            GameManager.Instance.EnnemiesLeft++;
            ennemiesLeft++;
        }
        else
        {
            GameManager.Instance.WaveNumber++;
            return;
        }

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
