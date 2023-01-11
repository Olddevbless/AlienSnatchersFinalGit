using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] float destroyAfter; // set the time after which the projectile should be destroyed

    
    void Start()
    {
        // start a coroutine to destroy the projectile after the specified time
        StartCoroutine(DestroyAfter());
    }

    
    IEnumerator DestroyAfter()
    {
        // wait for the specified time
        yield return new WaitForSeconds(destroyAfter);

        // destroy the projectile
        Destroy(gameObject);
    }
   

}



