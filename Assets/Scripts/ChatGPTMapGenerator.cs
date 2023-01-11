using System.Collections.Generic;
using UnityEngine;




public class ChatGPTMapGenerator : MonoBehaviour
{
    [Header("Map Colors")]
    public Texture2D whiteTexture;
    public Texture2D brownTexture;
    public Texture2D greenTexture;
    public Texture2D blueTexture;
    [SerializeField] SplatPrototype[] splatPrototypes;

    [Header("Noise Map Generation")]
    public int mapWidth;
    public int mapHeight;
    public int seed;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
    public Vector2 offset;
    [Header("Details generation")]
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public Terrain terrain;

    [Header("Debugging")]
    int pointlocationPrinted;


    private void Awake()
    {
        offset = new Vector2(mapWidth / 4, mapHeight / 4);
    //    //mapWidth = Random.Range(200, 500);
    //    //mapHeight = Random.Range(200, 500);
    //    //seed = Random.Range(0, 100);
    //    //scale = Random.Range(5, 50);
    //    //octaves = Random.Range(10, 100);
    //    //persistance = Random.Range(0.1f, 0.5f);
    //    //lacunarity = 2f;
    //    //offset = new Vector2(Random.Range(100,1000), Random.Range(100, 1000));
        GenerateMap();
    
    }

    public void GenerateMap()
    {
        float[,] noiseMap = ChatGPTNoise.GenerateNoiseMap(mapWidth, mapHeight,seed, scale,octaves,persistance,lacunarity,offset);

        // Create a new terrain data object
        TerrainData terrainData = new TerrainData();
        SetTerrainSizeAndHeight(terrainData, mapWidth, mapHeight, noiseMap);
        //AddDetailsToTerrain(terrainData, rockPrefab, noiseMap);

        // Create a new terrain object using the terrain data object
        terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        SetTerrainTextures(terrainData, whiteTexture, brownTexture, greenTexture, blueTexture, noiseMap);
        AddTreesToTerrain(treePrefab, noiseMap, terrainData);
        terrain.tag = "Ground";

    }

    public void SetTerrainSizeAndHeight(TerrainData terrainData, int mapWidth, int mapHeight, float[,] noiseMap)
    {

        // Set the size of the terrain data object
        terrainData.heightmapResolution = mapHeight;
        terrainData.size = new Vector3(mapWidth, 300, mapHeight);

        // Set the heightmap of the terrain data object using the noise map
        terrainData.SetHeights(0, 0, noiseMap);
    }

    public void AddTreesToTerrain(GameObject treePrefab, float[,] noiseMap, TerrainData terrainData)
    {
        const float MIN_TREE_DISTANCE = 5.0f;

        List<GameObject> existingTrees = new List<GameObject>();

        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {

                if (noiseMap[x, y] > 0.8f&&noiseMap[x,y]<1 && existingTrees.Count < 1000)
                {
                    bool positionAvailable = true;
                    Vector3 treePosition = new Vector3(x, terrainData.GetHeight(x, y), y);

                    foreach (GameObject existingTree in existingTrees)
                    {
                        // Calculate the 2D distance between the current tree position and the position of the existing tree
                        float distance = Vector3.Distance(treePosition, existingTree.transform.position);

                        if (distance < MIN_TREE_DISTANCE)
                        {
                            positionAvailable = false;
                            break;
                        }
                    }

                    if (positionAvailable)
                    {
                        Quaternion treeRotation = Quaternion.identity;
                        GameObject tree = Instantiate(treePrefab, treePosition, treeRotation);
                        existingTrees.Add(tree);
                    }
                }
            }
        }
    }
    public void SetTerrainTextures(TerrainData terrainData, Texture2D whiteTexture, Texture2D brownTexture, Texture2D greenTexture, Texture2D blueTexture, float[,] noiseMap)
    {
        float[,] heightmap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        SplatPrototype[] splatPrototypes = new SplatPrototype[4];
        splatPrototypes[0] = new SplatPrototype
        {
            texture = whiteTexture
        };
        splatPrototypes[1] = new SplatPrototype
        {
            texture = brownTexture
        };
        splatPrototypes[2] = new SplatPrototype
        {
            texture = greenTexture
        };
        splatPrototypes[3] = new SplatPrototype
        {
            texture = blueTexture
        };

        terrainData.splatPrototypes = splatPrototypes;

        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {

                float[] splat = new float[terrainData.alphamapLayers];
                float height = heightmap[y, x];
                
                if (height < 0.5f)
                {
                    splat[0] = 1;
                }
                else if (height < 0.6f)
                {
                    splat[1] = 1;
                }
                else if (height < 0.7f)
                {
                    splat[2] = 1;
                }
                else
                {
                    splat[3] = 1;
                }

                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    splatmapData[x, y, i] = splat[i];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
    }


    //Deprecated methods
    //    public DetailPrototype CreateDetailPrototype(GameObject prefab, DetailRenderMode renderMode)
    //{
    //    // Create a new detail prototype
    //    DetailPrototype detailPrototype = new DetailPrototype();

    //    // Set the prefab and render mode for the prototype
    //    detailPrototype.prototype = prefab;
    //    detailPrototype.renderMode = renderMode;

    //    // Return the detail prototype
    //    return detailPrototype;
    //}
    //public void AddDetailsToTerrain(TerrainData terrainData, GameObject rockPrefab, float[,] noiseMap)

    //{
    //    // Create a detail prototype using the specified prefab
    //    DetailPrototype detailPrototype = CreateDetailPrototype(rockPrefab, DetailRenderMode.Grass);

    //    // Add the detail prototype to the terrain data
    //    terrainData.detailPrototypes = new DetailPrototype[] { detailPrototype };
    //    Debug.Log("Added " + terrainData.detailPrototypes.Length + " detail prototypes to the terrain data");

    //    // Create a detail layer for the terrain
    //    int[,] detailLayer = new int[mapWidth, mapHeight];

    //    // Populate the detail layer with the desired density of rocks
    //    for (int x = 0; x < mapWidth; x++)
    //    {
    //        for (int y = 0; y < mapHeight; y++)
    //        {
    //            detailLayer[x, y] = (int)(noiseMap[x, y] * 10);
    //        }
    //    }

    //    // Add the detail layer to the terrain data
    //    int detailResolution = 200;
    //    terrainData.SetDetailResolution(mapWidth, detailResolution);
    //    terrainData.SetDetailLayer(0, 0, 0, detailLayer);
    //}

}

