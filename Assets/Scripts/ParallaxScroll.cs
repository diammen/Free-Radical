using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public Transform cameraPos;
    public float speed;
    Vector3 lastPos;

    // Use this for initialization
    void Start()
    {
        cameraPos = Camera.main.transform;
        lastPos = cameraPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posVector = (lastPos - cameraPos.position) * speed;
        posVector = new Vector3(posVector.x, 0, posVector.y);
        transform.position -= posVector;
        lastPos = cameraPos.position;
    }
}