using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
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
    CheckTrigger wallCollider;
    [SerializeField]
    CheckTrigger hitCollider;
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
        wallCollider = GetComponentInChildren<CheckTrigger>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        sqrMaxSpeed = maxSpeed * maxSpeed;
    }

    void Update()
    {
        if (hitCollider.isColliding && hitCollider.collidedWith != null)
        {
            hitCollider.collidedWith.gameObject.SetActive(false);
            creaturesEaten++;
            rb.transform.localScale += new Vector3(currentFoodIntake, currentFoodIntake, currentFoodIntake);
            hitCollider.isColliding = false;
        }
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

        touchingWall = wallCollider.isColliding;

        //if (rb.transform.localScale.x > minSize)
        //    rb.transform.localScale -= new Vector3(sizeReductionRate, sizeReductionRate, sizeReductionRate) / 10 * Time.deltaTime;

        if (rb.transform.localScale.x > maxGrowthSize)
        {
            gm.reachedCheckpoint = true;
            minSize = maxGrowthSize;
            maxGrowthSize *= 2;
            currentFoodIntake *= 2;
        }
    }

    void ScaleWall()
    {
        rb.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
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