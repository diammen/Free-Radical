using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        Vector3 temp = transform.position - target.position;
        offset = new Vector3(temp.x, temp.y, temp.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, 0, target.position.z) + offset, 0.1f);

        if (Input.GetAxis("Horizontal") != 0) offset.x *= Input.GetAxis("Horizontal");
    }
}