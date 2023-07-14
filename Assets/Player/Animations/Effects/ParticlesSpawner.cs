using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticlesSpawner : MonoBehaviour
{
    [SerializeField] GameObject runParticles;
    public bool spawnParticles = false;
    private void Update()
    {
        if (spawnParticles)
        {
            SpawnParticles();
            spawnParticles = false;
        }
    }
    public void SpawnParticles()
    {
        Instantiate(runParticles, transform.position, Quaternion.identity);
    }
}
