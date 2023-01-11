using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMothership : MonoBehaviour
{
    public Radar radarScript;
    public GameObject mothershipPrefab;
    public GameObject alienAttackshipPrefab;
    public GameObject alienAttackshipHatch;
    public int poolSize = 10;
    public List<GameObject> objectPool;
    public float spawnInterval = 15.0f;

    private void Awake()
    {
        radarScript = FindObjectOfType<Radar>();
    }
    void Start()
    {
        
        // Initialize the object pool
        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject alienAttackShip = Instantiate(alienAttackshipPrefab, transform.position, Quaternion.identity);
            alienAttackShip.transform.SetParent(gameObject.transform, true);
            alienAttackShip.SetActive(false);
            objectPool.Add(alienAttackShip);
            

        }

        // Call the Spawn method repeatedly every spawnInterval seconds
        InvokeRepeating("Spawn", 15f, spawnInterval);
        
    }
    private void Update()
    {
        
    }

    public void Spawn()
    {
        // Find an inactive object in the object pool
        GameObject alienAttackShip = objectPool.Find(x => !x.activeInHierarchy);
        //radarScript.UpdateAlienAttackShipsList();
        
        // Activate the object and set its position and rotation
        if (alienAttackShip != null)
        {
            alienAttackShip.SetActive(true);
            alienAttackShip.transform.position = alienAttackshipHatch.transform.position;
            alienAttackshipHatch.transform.rotation = Quaternion.identity;
        }
    }
}







