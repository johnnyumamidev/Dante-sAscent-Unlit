using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundGeneration : MonoBehaviour
{
    public int width, height;
    [SerializeField] int minHeight, maxHeight;
    [SerializeField] int previousHeight;
    [SerializeField] int maxHeightVariance;
    [SerializeField] int numberOfRepeats, minLength, maxLength;
    [SerializeField] int ceilingMin, ceilingMax;
    [SerializeField] GameObject tile;
    [SerializeField] GameObject wallTile;
    public Transform groundStart;
    public Transform ceilingStart;

    public int wallHeight;

    [SerializeField] Sprite floorSprite;
    [SerializeField] Sprite ceilingSprite;
    [SerializeField] Sprite ceilingLight;

    [SerializeField] GameObject gate;
    [SerializeField] int entranceHeight, exitHeight;
    int westGateYPos;
    int eastGateYPos;
    private void Start()
    {
        GenerateGround();
        GenerateCeiling();
        GenerateWalls();
    }

    private void GenerateCeiling()
    {
        int repeatValue = 0;
        for (int x = 0; x < width; x++)
        {
            if (repeatValue == 0)
            {
                numberOfRepeats = Random.Range(ceilingMin, ceilingMax);
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
                    else if (height >= maxHeight)
                    {
                        stepDirection = -1;
                    }
                    else if (height <= minHeight)
                    {
                        stepDirection = 1;
                    }
                    prevRoll = stepDirection;
                    int step = Random.Range(1, maxHeightVariance);
                    step *= stepDirection;
                    height = previousHeight + step;
                    previousHeight = height;
                }
                SpawnCeilingTiles(x, ceilingStart);
                repeatValue = numberOfRepeats;
            }
            else
            {
                SpawnCeilingTiles(x, ceilingStart);
                repeatValue--;
            }
        }
    }

    private void GenerateWalls()
    {
        Vector2 start = groundStart.position;
        for (int y = 0; y < wallHeight; y++)
        {
            Vector2 entrancePosition = new Vector2(start.x-1, start.y + y);
            GameObject entrance = Instantiate(wallTile, entrancePosition, Quaternion.identity, transform);
            entrance.name = "Wall";
            Vector2 exitPosition = new Vector2(width + start.x, start.y + y);
            GameObject exit = Instantiate(wallTile, exitPosition, Quaternion.identity, transform);
            exit.transform.localScale = new Vector3(-exit.transform.localScale.x, exit.transform.localScale.y, exit.transform.localScale.z);
            exit.name = "Wall";
            if (y == exitHeight - 1) eastGateYPos = (int)exitPosition.y;
            if (y==entranceHeight-1) westGateYPos = (int)entrancePosition.y;
        }
        
        for (int i = 0; i < 2; i++)
        {
            Vector2 pos = Vector2.zero;
            if (i == 0) pos = new Vector2(start.x - 1, westGateYPos + 0.5f);
            else { pos = new Vector2(start.x + width, eastGateYPos + 0.5f); } 
            Instantiate(gate, pos, Quaternion.identity);
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
                    entranceHeight = height;
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
                SpawnGroundTiles(x, groundStart);
                repeatValue = numberOfRepeats;
            }
            else
            {
                SpawnGroundTiles(x, groundStart);
                repeatValue--;
            }

            if (x == width - 1)
            {
                exitHeight = height;
            }
        }
    }

    private void SpawnCeilingTiles(int x, Transform startPoint)
    {
        Vector2 start = startPoint.position;
        for (int y = 0; y < height; y++)
        {
            Vector2 position = new Vector2(start.x + x, start.y - y);
            GameObject ceilingTile = Instantiate(tile, position, Quaternion.identity, transform);
            ceilingTile.name = "Ground";
            if (y == height - 1)
            {
                ceilingTile.name = "Ceiling";
                ceilingTile.transform.localScale = new Vector3(ceilingTile.transform.localScale.x, -ceilingTile.transform.localScale.y, ceilingTile.transform.localScale.z);
                Transform light = ceilingTile.GetComponent<Tile>().lighting;
                light.GetComponent<SpriteRenderer>().sprite = ceilingLight;
                light.gameObject.SetActive(true);
                SpriteRenderer sr = ceilingTile.GetComponent<SpriteRenderer>();
                sr.sprite = ceilingSprite;
                sr.color = Color.white;
            }
        }
    }
    private void SpawnGroundTiles(int x, Transform startPoint)
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
