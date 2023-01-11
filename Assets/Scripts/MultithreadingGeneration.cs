//using System.Threading;
//using Unity;
//using UnityEngine;
//public class ChatGPTMapGenerator : MonoBehaviour
//{
//    // Other code...

//    public void GenerateMap()
//    {
//        // Create a new terrain data object
//        TerrainData terrainData = new TerrainData();
//        terrainData.heightmapResolution = mapHeight;
//        terrainData.size = new Vector3(mapWidth, 300, mapHeight);

//        // Generate the noise map on a separate thread
//        Thread noiseThread = new Thread(() =>
//        {
//            float[,] noiseMap = ChatGPTNoise.GenerateNoiseMap(mapWidth, mapHeight, seed, scale, octaves, persistance, lacunarity, offset);
//            // Set the heightmap of the terrain data object using the noise map
//            terrainData.SetHeights(0, 0, noiseMap);
//        });

//        // Start the thread to generate the noise map
//        noiseThread.Start();

//        // Generate the terrain on the main thread
//        terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
//        AddTreesToTerrain(treePrefab, terrainData);
//        terrain.tag = "Ground";
//    }

//    // Other code...
//}
