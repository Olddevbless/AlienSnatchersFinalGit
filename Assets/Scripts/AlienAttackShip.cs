using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAttackShip : MonoBehaviour
{
    public float boostSpeed = 5.0f;
    public float radius =5f;
    public float fireRate;
    public GameObject projectilePrefab;
    private Rigidbody rb;
    private Transform player;
    private float timeSinceLastShot;
    private float boostTimer;
    public float boostCooldown= 5f;
    private bool isBoosting = true;
    public GameObject terrain;
    float distanceFromGround;
    [SerializeField] GameObject miniMapSprite;
    GameManager gameManager;
    AlienMothership alienMothership;
    [SerializeField] GameObject explosionVFX;
    AudioSource audioSource;
    [SerializeField] AudioClip passByClip;
    bool audioHasNotBeenPlayed;
    float audioTimer;
    void Start()
    {
        fireRate= Random.Range(0.2f, 0.7f);
        audioSource = GetComponent<AudioSource>();
        alienMothership = FindObjectOfType<AlienMothership>();
        gameManager = FindObjectOfType<GameManager>();
        terrain = FindObjectOfType<PerlinNoiseMapChatGPT>().gameObject;
        rb = GetComponent<Rigidbody>();
        // Find the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isBoosting = true;
        
    }
    void Update()
    {
        miniMapSprite.transform.position = new Vector3(transform.position.x, 50, transform.position.z);
        miniMapSprite.transform.rotation = Quaternion.Euler(90, 0, 0);
        Vector3 direction = Vector3.RotateTowards(transform.position, player.position, 180, 100);
        // Calculate the distance between the object and the player
        float distance = Vector3.Distance(transform.position, player.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            distanceFromGround = hit.distance;
        }
        PlayFlyBySFX();
        timeSinceLastShot += Time.deltaTime;
        Bounds terrainBounds = terrain.GetComponent<Renderer>().bounds;
        OutOfBounds(terrainBounds);
        if (timeSinceLastShot >= fireRate)
        {
            // Fire a projectile and reset the timer
            FireProjectile();
            timeSinceLastShot = 0;
        }

        // Check if the distance from the ground is less than the threshold
        if (distanceFromGround < 10)
        {
            // Stop the ship's movement
            rb.velocity = Vector3.zero;

            // Wait for a specified delay before moving towards the player
            StartCoroutine(WaitAndMoveTowardsPlayer(1.0f));
        }
        else
        {
            RotateAndMoveTowardsPlayer(distance);
        }
    }
    void RotateAndMoveTowardsPlayer(float distance)
    {
        // Calculate the direction to the player
        Vector3 direction = player.position - transform.position;

        // Rotate the ship towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5.0f);

        if (distance > radius && distance < radius * 2)
        {
            // The ship is within the radius, but not too close to the player
            // Stop the ship's movement
            rb.velocity = Vector3.zero;
        }
        else if (distance > radius * 2)
        {
            // The ship is outside the radius, move towards the player
            StartCoroutine(MoveTowardsPlayer(Vector3.forward));
        }
    }

    IEnumerator WaitAndMoveTowardsPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(MoveTowardsPlayer(Vector3.forward));
    }

    void FireProjectile()
    {
        // Calculate the direction to a point slightly ahead of the player's position
        Vector3 target = player.position + player.forward * 5;
        Vector3 direction = (target - transform.position).normalized;

        // Instantiate the projectile and set its direction
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectile.GetComponent<EnemyProjectile>().speed;
    }
    IEnumerator MoveTowardsPlayer(Vector3 direction)
    {
        // Move towards the player
        rb.AddForce(direction * boostSpeed, ForceMode.Impulse);
        isBoosting = false;
        yield return new WaitForSeconds(boostCooldown);
        isBoosting = true;
        

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            gameManager.AddPoints(1);
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Debug.Log("I've been hit");
            transform.position = alienMothership.alienAttackshipHatch.transform.position;
            gameObject.SetActive(false);
        }
    }
    void PlayFlyBySFX()
    {
        audioTimer = +Time.deltaTime;
            if (audioTimer > 5)
            {
                audioSource.PlayOneShot(passByClip);
                audioTimer = 0;
            }
    }
    public void OutOfBounds(Bounds terrainBounds)
    {

        // Limit the player's position to within the bounds of the terrain
        if (transform.position.x < terrainBounds.min.x)
        {
            transform.position = new Vector3(terrainBounds.min.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x > terrainBounds.max.x)
        {
            transform.position = new Vector3(terrainBounds.max.x, transform.position.y, transform.position.z);
        }
        if (transform.position.z < terrainBounds.min.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, terrainBounds.min.z);
        }
        if (transform.position.z > terrainBounds.max.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, terrainBounds.max.z);
        }
    }

}
