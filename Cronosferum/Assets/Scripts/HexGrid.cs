using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab;

    //Size of the map in terms of the number of hex tiles
    public int width = 20;
    public int height = 20;

    //Distance between 2 rows of tiles
    //This will be half on odd rows beaceasue of the hex form
    float xOffset = 0.882f;

    //Distance between 2 hex tiles
    //If this is missing, tilles will overlap
    float zOffset = 0.764f;

    //Perlin values
    public float perlinScale = 20f;
    float seed;

    // Use this for initialization
    void Start()
    {
        this.GenerateGrid();
    }

    void GenerateGrid()
    {
        seed = Random.Range(0f, 99999f);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float xPos = x * xOffset;

                //Are we on an odd row?
                if (z % 2 == 1)
                {
                    xPos += xOffset / 2;
                }

                var newHex = Instantiate(hexPrefab, new Vector3(xPos, 0, z * zOffset), Quaternion.identity);
                this.GenerateTerrain(newHex, x, z);
            }
        }
    }

    void GenerateTerrain(GameObject hex, int xPos, int zPos)
    {
        int perlinValue = this.CalculatePerlinValue(xPos, zPos);
        Color color = CalculateColor(xPos, zPos);
        var hexModel = hex.GetComponent<HexCell>();
        var hexRenderer = hex.GetComponentInChildren<Renderer>();
        hexModel.setHeight(perlinValue);
        hexRenderer.material.SetColor("_Color", color);
    }



    int CalculatePerlinValue(int x, int z)
    {
        float xPerlinCoord = (float)x / 256 * perlinScale + seed;
        float zPerlinCoord = (float)z / 256 * perlinScale + seed;

        float sample = Mathf.PerlinNoise(xPerlinCoord, zPerlinCoord);
        if (sample < 0.35f)
        {
            return 1;
        }
        else if (sample >= 0.35f && sample < 0.60f)
        {
            return 3;
        }
        else if (sample >= 0.60f && sample <= 1)
        {
            return 5;
        }
        return 1;
    }

    Color CalculateColor(int x, int z)
    {
        float xPerlinCoord = (float)x / 256 * perlinScale + seed;
        float zPerlinCoord = (float)z / 256 * perlinScale + seed;

        float sample = Mathf.PerlinNoise(xPerlinCoord, zPerlinCoord);
        if (sample < 0.35f)
        {
            sample += 0.1f;
            return new Color32(getPerlinColor(102, sample), getPerlinColor(153, sample), getPerlinColor(255, sample), 20);
        }
        else if (sample >= 0.35f && sample < 0.60f)
        {
            return new Color32(getPerlinColor(0, sample), getPerlinColor(204, sample), getPerlinColor(0, sample), 255);
        }
        else if (sample >= 0.60f && sample <= 1)
        {
            return new Color32(getPerlinColor(204, sample), getPerlinColor(102, sample), getPerlinColor(0, sample), 255);
        }
        return new Color32(102, 153, 255, 20);
    }


    byte getPerlinColor(byte colorValue, float perlinSample)
    {
        return (byte)((perlinSample * colorValue) + (perlinSample * colorValue) * 0.2f);
    }

    /*
	Color CalculateColor(int perlinValue)
    {
        if (perlinValue == 1)
        {
            return WATER;
        }
        if (perlinValue == 3)
        {
            return FOREST;
        }
        if (perlinValue == 5)
        {
            return MOUNTAIN;
        }
        return FOREST;
    }*/
}
