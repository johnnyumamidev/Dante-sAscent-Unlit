using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] GroundGeneration groundGenerator;

    [SerializeField] int distanceBetweenSpawnLocations;
    [SerializeField] int spawnCount;
    List<Vector2> spawnLocations = new List<Vector2>();
    void Start()
    {
        distanceBetweenSpawnLocations = Mathf.RoundToInt(groundGenerator.width / spawnCount);
        for(int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnLoc = new Vector2(distanceBetweenSpawnLocations * i, 0);
            Instantiate(enemyPrefab, spawnLoc, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
