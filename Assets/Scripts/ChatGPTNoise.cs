using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPTNoise : MonoBehaviour
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //System.Random prng = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = Random.Range(-100, 100) + offset.x;
            float offsetY = Random.Range(-100, 100) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 100f;
        }

        // Calculate the noise map
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x / scale * frequency) + octaveOffsets[i].x;
                    float sampleY = (y / scale * frequency) + octaveOffsets[i].y;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY); // Return Perlin noise value as is
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize the noise map
        float maxNoiseHeight = float.MinValue;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                maxNoiseHeight = Mathf.Max(maxNoiseHeight, noiseMap[x, y]);
            }
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] /= maxNoiseHeight;
            }
        }

        // Return the normalized noise map
        return noiseMap;
    }

}
