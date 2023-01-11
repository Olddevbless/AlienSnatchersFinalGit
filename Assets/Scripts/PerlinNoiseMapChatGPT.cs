using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
public class PerlinNoiseMapChatGPT : MonoBehaviour
{
    // The resolution of the perlin noise map
    public int mapResolution = 256;
    public int mapScale;
    // The scale of the perlin noise
    public float noiseScale = 20f;

    // The height multiplier for the terrain
    public float heightMultiplier = 20f;
    
    // The number of vertices on the x and z axes
    public int terrainWidth = 256;
    public int terrainHeight = 256;
    MeshRenderer meshRenderer;
    // The heightmap for the terrain
    private float[,] heightMap;
    public Gradient gradient;
    float maxTerrainHeight;
    float minTerrainHeight;
    Collider meshCollider;
    [SerializeField] GameObject[] treePrefabs;
    [SerializeField] Renderer textureRenderer;
    
    void Awake()
    {
        noiseScale = Random.Range(3f, 12f);
        heightMultiplier = Random.Range(25, 30);
        // Generate the perlin noise map
        heightMap = GeneratePerlinNoiseMap();
        meshCollider = GetComponent<MeshCollider>();
        // Generate the terrain
        GenerateTerrain();
        ColorTerrain();
        Create2DImage(heightMap);
        GenerateTrees();
        this.gameObject.tag = "Ground";
        
    }

    // Generates a perlin noise map using Unity's built-in Perlin Noise function
    private float[,] GeneratePerlinNoiseMap()
    {
        // Initialize the height map
        float[,] heightMap = new float[mapResolution, mapResolution];

        // Loop through each point on the map
        for (int y = 0; y < mapResolution; y++)
        {
            for (int x = 0; x < mapResolution; x++)
            {
                // Generate the perlin noise value for the current point
                float noiseValue = Mathf.PerlinNoise((float)x / mapResolution * noiseScale, (float)y / mapResolution * noiseScale);

                // Set the height map value for the current point
                heightMap[x, y] = noiseValue;
            }
        }

        return heightMap;
    }

    //Generates a terrain mesh from the perlin noise map
    private void GenerateTerrain()
    {
        // Create a new mesh and set it as the filter's mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Create the vertices for the terrain
        Vector3[] vertices = new Vector3[terrainWidth * terrainHeight];
        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                // Calculate the vertex's position
                vertices[y * terrainWidth + x] = new Vector3(x, heightMap[x, y] * heightMultiplier, y);
                if (y > maxTerrainHeight)
                {
                    maxTerrainHeight = y;
                }
                if (y < minTerrainHeight)
                {
                    minTerrainHeight = y;
                }
            }
        }
        mesh.vertices = vertices;

        // Create the triangles for the terrain
        int[] triangles = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
        int triangleIndex = 0;
        for (int y = 0; y < terrainHeight - 1; y++)
        {
            for (int x = 0; x < terrainWidth - 1; x++)
            {
                triangles[triangleIndex] = (y * terrainWidth) + x;
                triangles[triangleIndex + 1] = ((y + 1) * terrainWidth) + x;
                triangles[triangleIndex + 2] = (y * terrainWidth) + x + 1;
                triangles[triangleIndex + 3] = ((y + 1) * terrainWidth) + x;
                triangles[triangleIndex + 4] = ((y + 1) * terrainWidth) + x + 1;
                triangles[triangleIndex + 5] = (y * terrainWidth) + x + 1;
                triangleIndex += 6;
            }
        }
        mesh.triangles = triangles;

        // Calculate the normals for the mesh
        mesh.RecalculateNormals();
        gameObject.transform.localScale *= mapScale;
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }
    // Create the triangles of the cube
    //private void GenerateTerrain()
    //{
    //    // Create a new mesh and set it as the filter's mesh
    //    Mesh mesh = new Mesh();
    //    GetComponent<MeshFilter>().mesh = mesh;

    //    // Initialize lists to store the vertices and triangles of the mesh
    //    List<Vector3> vertices = new List<Vector3>();
    //    List<int> triangles = new List<int>();

    //    // Loop through the voxel data
    //    for (int y = 0; y < terrainHeight; y++)
    //    {
    //        for (int x = 0; x < terrainWidth; x++)
    //        {
    //            for (int z = 0; z < mapResolution; z++)
    //            {
    //                // Skip empty voxels
    //                if (heightMap[x, y] <= 0)
    //                {
    //                    continue;
    //                }

    //                // Calculate the position of the voxel
    //                Vector3 position = new Vector3(x, y * heightMultiplier, z);

    //                // Calculate the perturbation to apply to the vertices using Perlin noise
    //                float perturbation = Mathf.PerlinNoise(position.x / noiseScale, position.z / noiseScale) * 2 - 1;

    //                // Add the voxel's position as a vertex
    //                vertices.Add(position + new Vector3(0, perturbation, 0));

    //                // Check if the voxel has any neighbors
    //                if (x < terrainWidth - 1 && heightMap[x + 1, y] > 0)
    //                {
    //                    // Add a triangle between the current voxel and its right neighbor
    //                    triangles.Add(vertices.Count - 1);
    //                    triangles.Add(vertices.Count - 2);
    //                    triangles.Add(vertices.Count - 3);
    //                }
    //                if (y < terrainHeight - 1 && heightMap[x, y + 1] > 0)
    //                {
    //                    // Add a triangle between the current voxel and its top neighbor
    //                    triangles.Add(vertices.Count - 1);
    //                    triangles.Add(vertices.Count - 3);
    //                    triangles.Add(vertices.Count - 4);
    //                }
    //                if (z < mapResolution - 1 && heightMap[x, y] > 0)
    //                {
    //                    // Add a triangle between the current voxel and its front neighbor
    //                    triangles.Add(vertices.Count - 1);
    //                    triangles.Add(vertices.Count - 4);
    //                    triangles.Add(vertices.Count - 5);
    //                }
    //            }
    //        }
    //    }

    //    // Assign the vertices and triangles to the mesh
    //    mesh.vertices = vertices.ToArray();
    //    mesh.triangles = triangles.ToArray();

    //    // Recalculate the normals of the mesh
    //    mesh.RecalculateNormals();
    //}
    // Colors the terrain mesh using a gradient based on the height of the terrain
    private void ColorTerrain()
    {
        // Get the mesh and its vertices
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // Create an array to hold the vertex colors
        Color[] colors = new Color[vertices.Length];

        // Create a gradient

        // Loop through each vertex and set its color based on the gradient
        for (int i = 0; i < vertices.Length; i++)
        {
            //float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);

            float height = vertices[i].y / heightMultiplier;
            colors[i] = gradient.Evaluate(height);
            
        }

        // Set the vertex colors for the mesh
        mesh.colors = colors;

    }
    private void Create2DImage(float[,] heightMap)
    {

        // Create a new texture with the desired width and height
        Texture2D texture = new Texture2D(mapResolution, mapResolution);

        // Set the texture's wrap mode to "Clamp"
        texture.wrapMode = TextureWrapMode.Clamp;

        // Set the color of the topographic lines
        Color lineColor = Color.black;

        // Set the thickness of the topographic lines
        int lineThickness = 2;

        // Set the height intervals at which the topographic lines should be drawn
        float[] heightIntervals = { 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };

        // Loop through each point in the height map
        for (int y = 0; y < mapResolution; y++)
        {
            for (int x = 0; x < mapResolution; x++)
            {
                // Calculate the color for the current point based on the gradient
                Color color = gradient.Evaluate(heightMap[x, y]);

                // Set the pixel color for the current point
                texture.SetPixel(x, y, color);

                // Check if the current point is a topographic line point
                if (y % lineThickness == 0)
                {
                    // Check if the current height is within one of the specified intervals
                    foreach (float heightInterval in heightIntervals)
                    {
                        if (heightMap[x, y] >= heightInterval && heightMap[x, y] < heightInterval + 0.1f)
                        {
                            // Draw a topographic line from the current point to the next point
                            for (int i = x; i < x + lineThickness; i++)
                            {
                                // Set the pixel color at the current point
                                texture.SetPixel(i, y, lineColor);
                            }
                            break;
                        }
                    }
                }
            }
        }

        // Apply the changes to the texture
        texture.Apply();

        // Assign the texture to the material's main texture
        textureRenderer.material.mainTexture = texture;
        textureRenderer.gameObject.transform.localScale *= mapResolution;
textureRenderer.gameObject.transform.localPosition = new Vector3(98, 0.5f, 100);
}


    private void GenerateTrees()
    {
        // Loop through each point on the map
        for (int y = 0; y < mapResolution; y++)
        {
            for (int x = 0; x < mapResolution; x++)
            {
                // Get the height of the terrain at this point
                float height = heightMap[x, y];

                // Check if the height is within a certain range
                //if (height >= 0.1f && height <= 0.2f)
                //{
                //    // Calculate the position to instantiate the grass at
                //    Vector3 treePosition = new Vector3(x*mapScale, height * heightMultiplier*mapScale, y* mapScale);

                //    //// Instantiate the grass
                //    Instantiate(treePrefabs[0], treePosition, Quaternion.identity);

                //}
                //if (height > 0.2f && height <= 0.3f)
                //{
                //    // Calculate the position to instantiate the rock at
                //    Vector3 treePosition = new Vector3(x * mapScale, height * heightMultiplier * mapScale, y * mapScale);

                //    //// Instantiate the rock
                //    Instantiate(treePrefabs[1], treePosition, Quaternion.identity);

                //}
                if (height > 0.3f && height <= 0.4f)
                {
                    // Calculate the position to instantiate the tree at
                    Vector3 treePosition = new Vector3(x * mapScale, height * heightMultiplier * mapScale, y * mapScale);

                    //// Instantiate the tree
                    Instantiate(treePrefabs[0], treePosition, Quaternion.identity);

                }
            }
        }
    }
    


}
