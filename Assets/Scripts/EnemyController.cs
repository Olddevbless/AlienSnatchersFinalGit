 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public float speed;
    public GameObject alienShip;
    public GameObject alienMothership;
    public GameObject humanPrefab;
    public GameManager gameManager;
    public SpawnManager spawnManager;
    public int health =1;
    bool humanSpawned;
    [SerializeField] GameObject miniMapSprite;
    [SerializeField] GameObject explosionVFX;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(5, 10);
        spawnManager = FindObjectOfType<SpawnManager>();
        humanSpawned = false;
        alienMothership = FindObjectOfType<AlienMothership>().gameObject;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        miniMapSprite.transform.position = new Vector3(transform.position.x, 50, transform.position.z);
        miniMapSprite.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(alienMothership.transform.position.x,alienMothership.transform.position.y+20,alienMothership.transform.position.z), speed * Time.deltaTime);
        //transform.Translate(Vector3.up*speed*Time.deltaTime);
        if (transform.position.y > 1000)
        {
            Destroy(gameObject);
            Debug.Log("You lost one!");
        }
       
    }
    

    private void OnDestroy()
    {
        // Remove the enemy from the enemy list when it is destroyed
        spawnManager.enemyList.Remove(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == alienMothership.name|| other.gameObject.CompareTag("AlienMotherShip"))
        {
            gameManager.RemoveLife();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            health--;
            Debug.Log("I've lost health");
        }

        if (health == 0)
        {
            if (humanSpawned == false)
            {
                // Instantiate a human game object at the enemy's position
                GameObject human=  Instantiate(humanPrefab, transform.position, transform.rotation,null);
                humanSpawned = true;
                human.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10)));
                // Disable the enemy's game object
                gameObject.GetComponent<SphereCollider>().enabled = false;
                alienShip.SetActive(false);
                Instantiate(explosionVFX, transform.position, Quaternion.identity);
                // Destroy the enemy game object after a few seconds
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
