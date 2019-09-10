using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed;
    public float patrolRange;
    public float moveTime;
    public bool canFlee;

    SpriteRenderer sprite;
    Rigidbody2D rb;
    Vector2 start;
    Vector3 destination;
    float moveTimer;

    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        destination = new Vector2(Random.Range(-patrolRange, patrolRange) + transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer > moveTime)
        {
            moveTimer = 0;
            destination = new Vector2(Random.Range(-10, 10) + transform.position.x, transform.position.y);
        }

        if (Vector2.Distance(transform.position, destination) < 0.1f)
        {
            destination = new Vector2(Random.Range(-10, 10) + transform.position.x, transform.position.y);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);

            Vector3 direction = destination - transform.position;

            sprite.flipX = direction.x < 0 ? false : true;
        }
    }
}