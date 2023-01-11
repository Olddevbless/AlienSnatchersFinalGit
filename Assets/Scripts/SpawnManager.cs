using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemiesToSpawn;
    [SerializeField] Radar radarScript;
    private int waveNumber = 1;
    public GameObject alienPrefab;
    public GameObject mothershipPrefab;
    [SerializeField] float spawnRange = 700;
    private int enemyCount;
    public Mesh terrainMesh;
    [SerializeField] PerlinNoiseMapChatGPT terrainScript;
    [SerializeField] int offset = 30;
    public List<GameObject> enemyList;
    [SerializeField] GameObject player;
    public float distanceToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        enemiesToSpawn = Random.Range(5, 10);
        radarScript = FindObjectOfType<Radar>();
        player = FindObjectOfType<PlayerController>().gameObject;
        enemyList = new List<GameObject>();
        //SpawnAlienWaves(enemiesToSpawn + waveNumber);
        Instantiate(mothershipPrefab, new Vector3(terrainScript.gameObject.GetComponent<Renderer>().bounds.center.x, 30 * terrainScript.mapScale, terrainScript.gameObject.GetComponent<Renderer>().bounds.center.x), Quaternion.identity);
        SpawnEnemies(enemiesToSpawn + waveNumber);

    }
    void Update()
    {
        int enemyCount = FindObjectsOfType<EnemyController>().Length;

        if (enemyCount == 0)
        {
            
            if (waveNumber>4)
            {
                int waveNumberMultiplier =+ waveNumber * 2;                
                SpawnEnemies(enemiesToSpawn + waveNumber);
            }
            else
            {
                waveNumber++;
                SpawnEnemies(enemiesToSpawn + waveNumber);
            }
            
        }
       
    }
    public Vector3 GetRandomTerrainPosition()
    {
        // Get the mothership GameObject
        GameObject mothership = FindObjectOfType<AlienMothership>().gameObject;

        // Get the collider of the terrain
        Collider collider = FindObjectOfType<PerlinNoiseMapChatGPT>().GetComponent<Collider>();

        // Generate a random angle around the mothership
        float angle = Random.Range(0f, 360f);

        // Calculate the x and z position at the given angle and distance from the mothership
        float x = mothership.transform.position.x + spawnRange * Mathf.Cos(angle);
        float z = mothership.transform.position.z + spawnRange * Mathf.Sin(angle);

        // Find the height of the terrain at the random x and z position using a Raycast
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, 500, z), Vector3.down, out hit, Mathf.Infinity))
        {
            float y = hit.point.y;
            return new Vector3(x, y, z);
        }
        else
        {
            // Return the original position if the Raycast did not hit the terrain
            return new Vector3(x, 0, z);
        }
    }


    private void SpawnEnemies(int count)
    {
        // Loop through and instantiate the specified number of enemies
        int spawned = 0;
        while (spawned < count)
        {
            // Generate a random position on the terrain
            Vector3 position = GetRandomTerrainPosition();

            // Instantiate an enemy at the random position
            GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);

            // Add the enemy to the list
            enemyList.Add(enemy);
            //radarScript.UpdateSnatcherList();

            // Increment the spawned counter
            spawned++;
        }
    }
   
}

   
    



