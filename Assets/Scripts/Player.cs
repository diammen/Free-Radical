using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject wallStuckTo;
    public int creaturesEaten;
    public float moveSpeed;
    public float maxSpeed;
    public float sizeReductionRate;
    public float maxGrowthSize;
    public float minSize;
    public float currentFoodIntake;
    public bool grappled;

    LineRenderer grappleRenderer;
    Rigidbody2D rb;
    CheckTrigger grappleCollider;
    GameManager gm;
    Vector2 currentVelocity;
    Vector2 force;
    Vector2 moveVector;
    Vector2 startPos;
    [SerializeField]
    float speed;
    [SerializeField]
    float horizontal;
    float timer;
    float sqrMaxSpeed;
    bool onGround;
    [SerializeField]
    bool touchingWall;

    // Use this for initialization
    void Start()
    {
        grappleRenderer = GetComponent<LineRenderer>();
        rb = GetComponentInChildren<Blob>().gameObject.GetComponent<Rigidbody2D>();
        grappleCollider = GetComponentInChildren<CheckTrigger>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        sqrMaxSpeed = maxSpeed * maxSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");

        moveVector = new Vector2(horizontal, 0) * (moveSpeed * 5000);
        currentVelocity = rb.velocity;

        force = moveVector * Time.deltaTime - currentVelocity;
        rb.AddForce(force, ForceMode2D.Force);

        if (currentVelocity.sqrMagnitude > sqrMaxSpeed)
        {
            rb.velocity = currentVelocity.normalized * maxSpeed;
        }

        speed = rb.velocity.sqrMagnitude;

        if (touchingWall && horizontal != 0)
            ScaleWall();

    }

    void ScaleWall()
    {
        rb.AddForce(Vector2.up * 750);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            touchingWall = true;
        }
        if (collision.gameObject.CompareTag("NPC"))
        {
            creaturesEaten++;
            rb.transform.localScale += new Vector3(currentFoodIntake, currentFoodIntake, currentFoodIntake);
            collision.gameObject.SetActive(false);
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