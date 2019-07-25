using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTrigger : MonoBehaviour {

    public bool isColliding;
    public LayerMask layers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (layers == (layers | (1 << collision.gameObject.layer))) isColliding = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (layers == (layers | (1 << collision.gameObject.layer))) isColliding = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
}
