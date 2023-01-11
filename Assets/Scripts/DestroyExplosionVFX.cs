using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosionVFX : MonoBehaviour
{
    [SerializeField] float vfxDuration;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,vfxDuration);
    }

    
}
