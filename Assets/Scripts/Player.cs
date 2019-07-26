using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject grapple;
    public GameObject wallStuckTo;
    public LayerMask grappleMask;
    public int creaturesEaten;
    public float grappleMaxDistance;
    public float moveSpeed;
    public float maxSpeed;
    public float sqrMaxSpeed;
    public float grappleSpeed;
    public float sizeReductionRate;
    public float maxGrowthSize;
    public float minSize;

    LineRenderer grappleRenderer;
    Rigidbody2D rb;
    CheckTrigger grappleCollider;
    GameManager gm;
    Vector2 currentVelocity;
    Vector2 force;
    Vector2 moveVector;
    Vector2 grapplePoint;
    Vector2 startPos;
    [SerializeField]
    float speed;
    [SerializeField]
    float horizontal;
    float timer;
    float grappleDistance;
    bool onGround;
    [SerializeField]
    bool touchingWall;
    bool grappled;

    // Use this for initialization
    void Start()
    {
        grappleRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
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

        ShootGrapple();
        RenderGrapple();

        if (grappled)
        {
            timer += Time.deltaTime;
            rb.position = Vector2.Lerp(startPos, grapplePoint, timer / Vector3.Distance(startPos, grapplePoint) * grappleSpeed);

            if (Vector3.Distance(rb.position, grapplePoint) <= 0.1f)
            {
                timer = 0;
                grappleRenderer.enabled = false;
                grappled = false;
                moveVector = Vector2.zero;
                rb.velocity = Vector2.zero;

                rb.gravityScale = 0;
            }
        }

        if (!grappleCollider.isColliding)
        {
            rb.gravityScale = 1;
        }

        if (rb.transform.localScale.x > minSize)
            rb.transform.localScale -= new Vector3(sizeReductionRate, sizeReductionRate, sizeReductionRate) / 10 * Time.deltaTime;

        if (rb.transform.localScale.x > maxGrowthSize)
        {
            gm.reachedCheckpoint = true;
            minSize = maxGrowthSize;
            maxGrowthSize *= 2;
        }

        grappleDistance = rb.transform.localScale.magnitude * grappleMaxDistance;
    }

    void ScaleWall()
    {
        rb.AddForce(Vector2.up * 750);
    }

    void ShootGrapple()
    {
        if (Input.GetMouseButtonUp(0) && !grappled)
        {

            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 grappleDir = (mousePoint - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, grappleDir, grappleDistance, grappleMask);

            if (hit.collider != null)
            {
                grappled = true;
                grappleRenderer.enabled = true;

                wallStuckTo = hit.collider.gameObject;
                startPos = rb.position;
                grapplePoint = hit.point;
            }
            else
            {
                grapplePoint = transform.position + grappleDir * grappleDistance;
                grappleRenderer.enabled = true;
                grappled = false;
            }

            float angle = Mathf.Atan2(grappleDir.y, grappleDir.x) * Mathf.Rad2Deg;

            grapple.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void RenderGrapple()
    {

        grappleRenderer.positionCount = 2;

        grappleRenderer.SetPosition(0, transform.position);
        grappleRenderer.SetPosition(1, grapplePoint);

        if (!grappled)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 grappleDir = (mousePoint - transform.position).normalized;

            grapplePoint = transform.position + grappleDir * grappleDistance;
        }
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
            rb.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
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