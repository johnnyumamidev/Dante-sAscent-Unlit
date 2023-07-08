using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    void Awake()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        int random = Random.Range(0, sprites.Count);
        spriteRenderer.sprite = sprites[random];
    }
}
