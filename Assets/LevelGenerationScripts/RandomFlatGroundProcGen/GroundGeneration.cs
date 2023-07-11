using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] int minHeight, maxHeight;
    [SerializeField] int numberOfRepeats, minLength, maxLength;
    [SerializeField] GameObject tile;
    [SerializeField] Transform startPosition;
    private void Start()
    {
        GenerateGround();
    }

    private void GenerateGround()
    {
        int repeatValue = 0;
        for (int x = 0; x < width; x++)
        {
            if (repeatValue == 0)
            {
                numberOfRepeats = Random.Range(minLength, maxLength);
                height = Random.Range(minHeight, maxHeight);
                Generate(x);
                repeatValue = numberOfRepeats;
            }
            else
            {
                Generate(x);
                repeatValue--;
            }
        }
    }

    private void Generate(int x)
    {
        Vector2 start = startPosition.position;
        for (int y = 0; y < height; y++)
        {
            Vector2 position = new Vector2(start.x + x,start.y + y);
            GameObject groundTile = Instantiate(tile, position, Quaternion.identity, transform);
            groundTile.name = "Tile";
        }
    }
}
