using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] checkpoints;
    public bool reachedCheckpoint;

    Player player;
    CameraFollow cameraFollow;
    int currentCheckpoint;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (reachedCheckpoint)
        {
            if (currentCheckpoint > checkpoints.Length)
                currentCheckpoint = checkpoints.Length;
            checkpoints[currentCheckpoint].gameObject.SetActive(false);
            currentCheckpoint++;

            cameraFollow.newCameraSize *= 1.5f;

            reachedCheckpoint = false;
        }
    }
}