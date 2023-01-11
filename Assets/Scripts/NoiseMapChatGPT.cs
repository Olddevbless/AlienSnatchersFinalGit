using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class NoiseMapChatGPT : MonoBehaviour

{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, Vector2 offset, int octaves, float persistence, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; o++)
                {
                    float sampleX = (x - mapWidth / 2) / scale * frequency + offset.x;
                    float sampleY = (y - mapHeight / 2) / scale * frequency + offset.y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        return noiseMap;
    }
}
