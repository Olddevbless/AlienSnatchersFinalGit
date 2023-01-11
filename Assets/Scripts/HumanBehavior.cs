using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBehavior : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public GameObject humanModel;
    [SerializeField] Collider groundCollider;
    [SerializeField] Collider netCollider;
    [SerializeField] Rigidbody rootRB;
    [SerializeField] AudioClip wilhelmScreamClip;
    [SerializeField] float timeInAir;
    [SerializeField] GameObject explosionVFX;
    // Start is called before the first frame update
    void Start()
    {
        
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = wilhelmScreamClip;
        audioSource.Play();
        gameManager = FindObjectOfType<GameManager>();
        //rootRB.AddTorque(new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3)));
        Physics.IgnoreLayerCollision(7, 10);
    }
    private void Update()
    {
        timeInAir += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    
    {
        
        if (other.gameObject.CompareTag("Ground"))
        {
            if (timeInAir>1)
            {
                gameManager.RemoveLife();
                Debug.Log("I have collided with ground");
                Instantiate(explosionVFX, transform.position,Quaternion.identity);
                Destroy(gameObject);
                
            }
            else
            {
                gameManager.AddPoints(1);

                Destroy(gameObject);
            }

        }

        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.AddPoints(1);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Net"))
        {
            gameManager.AddPoints(1);
            Destroy(gameObject);
        }
    }

}
