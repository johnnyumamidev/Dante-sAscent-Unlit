using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGeneration : MonoBehaviour
{
    [SerializeField] GroundGeneration ground;
    [SerializeField] SpriteRenderer texture;
    [SerializeField] SpriteRenderer shadow;

    void Start()
    {
        SetSpriteSize(texture);
        SetSpriteSize(shadow);
    }

    private void SetSpriteSize(SpriteRenderer sr)
    {
        sr.size = new Vector2(ground.width + 3, ground.wallHeight);
        float xPosition = ground.groundStart.position.x + (ground.width / 2);
        float yPosition = ground.groundStart.position.y + (ground.wallHeight / 2);
        sr.transform.position = new Vector2(xPosition, yPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
