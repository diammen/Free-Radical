using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject grapple;
    public float grappleMaxDistance;
    public float moveSpeed;
    public float jumpForce;

    Rigidbody2D rb;
    Vector2 currentVelocity;
    Vector2 force;
    Vector2 moveVector;
    [SerializeField]
    float speed;
    [SerializeField]
    float horizontal;
    bool onGround;
    [SerializeField]
    bool touchingWall;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        moveVector = new Vector2(horizontal, 0) * (moveSpeed * 100);

        currentVelocity = rb.velocity;

        force = moveVector * Time.deltaTime - currentVelocity;

        rb.AddForce(force, ForceMode2D.Force);

        speed = rb.velocity.sqrMagnitude;

        if (touchingWall && horizontal != 0)
            ScaleWall();

        ShootGrapple();
    }

    void ScaleWall()
    {
        rb.AddForce(Vector2.up * 25);
    }

    void ShootGrapple()
    {
        if (Input.GetMouseButtonUp(0))
        {

            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = (mousePoint - transform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, grappleMaxDistance);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            grapple.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            touchingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            touchingWall = false;
        }
    }
}