using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Radar : MonoBehaviour
{
    public SpawnManager spawnManager; // Reference to the SpawnManager script
    public AlienMothership alienMothership;
    List<GameObject> snatcherList;
    List<GameObject> alienAttackShipsList;
    public Canvas radarCanvas;
   
    //private void Start()
    //{
    //    spawnManager = FindObjectOfType<SpawnManager>();



    //}
    //void Update()
    //{
    //    if (GetComponentInChildren<Canvas>() != null)
    //    {
    //        if (alienMothership == null)
    //        {
    //            alienMothership = FindObjectOfType<AlienMothership>();
    //        }
    //        if (playerMinimapImage.gameObject == null)
    //        {
    //            Instantiate(playerMinimapImage);
    //        }
    //        timer += Time.deltaTime;
    //        if (timer >= interval)
    //        {
    //            timer = 0f;
    //            UpdateMinimap();
    //            // Calculate distance to each alien attack ship
    //            foreach (GameObject alienAttackShip in alienAttackShipsList)
    //            {
    //                float distance = Vector2.Distance(alienAttackShip.transform.position, playerMinimapImage.transform.position);

    //            }

    //            // Calculate distance to each snatcher
    //            foreach (GameObject snatcher in snatcherList)
    //            {
    //                float distance = Vector2.Distance(new Vector2(snatcher.transform.position.x, snatcher.transform.position.y), new Vector2(playerMinimapImage.transform.position.x, playerMinimapImage.transform.position.y));

    //            }
    //        }
    //    }

    //}
    //public void UpdateAlienAttackShipsList()
    //{
    //    alienAttackShipsList = alienMothership.objectPool;
    //}
    //public void UpdateSnatcherList()
    //{
    //    snatcherList = spawnManager.enemyList;
    //}
    //void UpdateMinimap()
    //{
    //    UpdateAlienAttackShipsList();
    //    UpdateSnatcherList();

    //    // Clear the minimap of any existing enemy minimap images
    //    foreach (Transform child in transform)
    //    {
    //        if (child.tag == "EnemyMinimapImage")
    //        {
    //            Destroy(child.gameObject);
    //        }
    //    }

    //    // Calculate and display the minimap image for each alien attack ship
    //    foreach (GameObject alienAttackShip in alienAttackShipsList)
    //    {
    //        // Calculate the distance and angle between the alien attack ship and the player
    //        float distance = Vector2.Distance(alienAttackShip.transform.position, playerMinimapImage.transform.position);
    //        float angle = Vector2.SignedAngle(Vector2.up, alienAttackShip.transform.position - playerMinimapImage.transform.position);

    //        // Calculate the minimap image's position relative to the player based on the distance and angle
    //        Vector3 minimapPosition = new Vector3(distance * Mathf.Cos(angle * Mathf.Deg2Rad), distance * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

    //        // Instantiate a new enemy minimap image prefab and position it on the minimap
    //        Image enemyMinimapImage = Instantiate(alienAttackShipImagePrefab, radarCanvas.transform);
    //        enemyMinimapImage.transform.localPosition = minimapPosition;
    //        enemyMinimapImage.transform.localScale = new Vector3(1, 1, 1);
    //    }

    //    // Calculate and display the minimap image for each snatcher
    //    foreach (GameObject snatcher in snatcherList)
    //    {
    //        // Calculate the distance and angle between the snatcher and the player
    //        float distance = Vector2.Distance(new Vector2(snatcher.transform.position.x, snatcher.transform.position.y), new Vector2(playerMinimapImage.transform.position.x, playerMinimapImage.transform.position.y));
    //        float angle = Vector2.SignedAngle(Vector2.up, snatcher.transform.position - playerMinimapImage.transform.position);

    //        // Calculate the minimap image's position relative to the player based on the distance and angle
    //        Vector3 minimapPosition = new Vector3(distance * Mathf.Cos(angle * Mathf.Deg2Rad), distance * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

    //        // Instantiate a new enemy minimap image prefab and position it on the minimap
    //        Image enemyMinimapImage = Instantiate(snatcherImagePrefab, radarCanvas.transform);
    //        enemyMinimapImage.transform.localPosition = minimapPosition;
    //        enemyMinimapImage.transform.localScale = new Vector3(1, 1, 1);
    //    }
    //}
}




