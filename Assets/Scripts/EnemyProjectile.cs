using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10.0f;
    public int damage = 10;
    public float lifetime = 5.0f;
    AudioSource audioSource;
    [SerializeField]AudioClip[] passByClip;
    PlayerController player;
    bool audioHasNotPlayed = true;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        // Destroy the projectile after a specified lifetime
        Destroy(gameObject, lifetime);
        Debug.Log("Fired");
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 5 || audioHasNotPlayed == true)
        {
            audioSource.PlayOneShot(passByClip[Random.Range(0,2)]);
            audioHasNotPlayed = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the projectile collided with the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            gameManager.TakeDamage(damage);
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
   
}
