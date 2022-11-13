using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEnemySpawner : MonoBehaviour
{

    [SerializeField] private int spawnInterval = 10; // Time between enemy spawns in seconds.
    [SerializeField] private GameObject enemyPrefab; // Enemy prefab to spawn.
    [SerializeField] private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

}
