using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    ContainerRandomizer container;
    ContainerData containerData;
    private void Awake()
    {
        container = GetComponent<ContainerRandomizer>();
        containerData = container.container;
    }
    public void SpawnCollectable()
    {
        Debug.Log("spawn random collectable item");
    }
}
