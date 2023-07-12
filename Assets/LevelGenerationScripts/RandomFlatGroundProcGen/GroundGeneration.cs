using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGeneration : MonoBehaviour
{
    public int width, height;
    [SerializeField] int minHeight, maxHeight;
    [SerializeField] int previousHeight;
    [SerializeField] int maxHeightVariance;
    [SerializeField] int numberOfRepeats, minLength, maxLength;
    [SerializeField] GameObject tile;
    [SerializeField] GameObject wallTile;
    public Transform groundStart;
    public Transform ceilingStart;

    public int wallHeight;

    [SerializeField] Sprite floorSprite;
    private void Start()
    {
        GenerateGround();
        GenerateWalls();
    }

    private void GenerateCeiling()
    {
        int repeatValue = 0;
        for (int x = 0; x < width; x++)
        {
            if (repeatValue == 0)
            {
                numberOfRepeats = Random.Range(minLength, maxLength);
                if (x == 0)
                {
                    height = Random.Range(minHeight, maxHeight);
                    previousHeight = height;
                }
                else
                {
                    int random = Random.Range(-1, 2);
                    if (random == 0) random = 1;
                    int step = Random.Range(1, maxHeightVariance);
                    step *= random;
                    height = previousHeight + step;
                    previousHeight = height;
                }
                Generate(x, ceilingStart);
                repeatValue = numberOfRepeats;
            }
            else
            {
                Generate(x, ceilingStart);
                repeatValue--;
            }
        }
    }

    private void GenerateWalls()
    {
        Vector2 start = groundStart.position;
        for (int y = 0; y < wallHeight; y++)
        {
            Vector2 westPosition = new Vector2(start.x-1, start.y + y);
            Vector2 eastPosition = new Vector2(width + start.x, start.y + y);
            GameObject westWall = Instantiate(wallTile, westPosition, Quaternion.identity, transform);
            GameObject eastWall = Instantiate(wallTile, eastPosition, Quaternion.identity, transform);
            eastWall.transform.localScale = new Vector3(-eastWall.transform.localScale.x, eastWall.transform.localScale.y, eastWall.transform.localScale.z);
            eastWall.name = "Wall";
            westWall.name = "Wall";
        }
    }
    int prevRoll;

    private void GenerateGround()
    {
        int repeatValue = 0;
        for (int x = 0; x < width; x++)
        {
            if (repeatValue == 0)
            {
                numberOfRepeats = Random.Range(minLength, maxLength);
                if (x == 0)
                {
                    height = Random.Range(minHeight, maxHeight);
                    previousHeight = height;
                }
                else
                {
                    int stepDirection = Random.Range(-1, 2);
                    if (stepDirection == 0)
                    {
                        if (prevRoll == 1) stepDirection = -1;
                        else { stepDirection = 1; }
                    }
                    else if(height >= maxHeight)
                    {
                        stepDirection = -1;
                    }
                    else if(height <= minHeight)
                    {
                        stepDirection = 1;
                    }
                    prevRoll = stepDirection;
                    int step = Random.Range(1, maxHeightVariance);
                    step *= stepDirection;
                    height = previousHeight + step;
                    previousHeight = height;
                }
                Generate(x, groundStart);
                repeatValue = numberOfRepeats;
            }
            else
            {
                Generate(x, groundStart);
                repeatValue--;
            }
        }
    }

    private void Generate(int x, Transform startPoint)
    {
        Vector2 start = startPoint.position;
        for (int y = 0; y < height; y++)
        {
            Vector2 position = new Vector2(start.x + x,start.y + y);
            GameObject groundTile = Instantiate(tile, position, Quaternion.identity, transform);
            groundTile.name = "Ground";
            if(y == height - 1)
            {
                groundTile.name = "Floor";
                Transform floorLight = groundTile.GetComponent<Tile>().lighting;
                floorLight.gameObject.SetActive(true);
                SpriteRenderer sr = groundTile.GetComponent<SpriteRenderer>();
                sr.sprite = floorSprite;
                sr.color = Color.white;
            }
        }
    }
}
