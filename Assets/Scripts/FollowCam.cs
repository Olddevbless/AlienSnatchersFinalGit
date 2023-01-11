using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    GameObject player;
    float bias = 0.96f;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveCamTo = player.transform.position - player.transform.forward *2f + Vector3.up * 3;
        transform.position = transform.position * bias + moveCamTo * (1.0f - bias);
        transform.LookAt(player.transform.position + player.transform.forward * 15f);
    }
}
