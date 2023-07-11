using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerRandomizer : MonoBehaviour
{
    [SerializeField] List<ContainerData> containers = new List<ContainerData>();
    public ContainerData container;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        int random = Random.Range(0, containers.Count);
        container = containers[random];
        spriteRenderer.sprite = container.sprite;
        transform.name = container.containerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
