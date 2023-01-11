using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashCamera : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        mainCamera.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
        if (player.activeInHierarchy == false)
        {
            transform.Translate(Vector3.up);
        }
    }
}
