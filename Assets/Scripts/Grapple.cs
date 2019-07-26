using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public GameObject hingeAnchor;
    public DistanceJoint2D joint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public Player player;
    private bool grappleAttached;
    private Vector2 playerPosition;
    private Rigidbody2D hingeAnchorRb;
    private SpriteRenderer hingeAnchorSprite;

    public LineRenderer grappleRenderer;
    public LayerMask grappleLayerMask;
    private float grappleMaxCastDistance = 50f;
    private List<Vector2> grapplePositions = new List<Vector2>();

    private bool distanceSet;

    // Use this for initialization
    void Start()
    {
        joint.enabled = false;
        playerPosition = transform.position;
        hingeAnchorRb = hingeAnchor.GetComponent<Rigidbody2D>();
        hingeAnchorSprite = hingeAnchor.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = worldMousePosition - transform.position;
        float aimAngle = Mathf.Atan2(direction.y, direction.x);

        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        playerPosition = transform.position;

        if (!grappleAttached)
        {
            SetCrosshairPosition(aimAngle);
        }
        else
        {
            crosshairSprite.enabled = false;
        }

        HandleInput(aimDirection);
    }

    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetMouseButton(0))
        {
            if (grappleAttached) return;
            grappleRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, grappleMaxCastDistance, grappleLayerMask);

            Debug.DrawRay(playerPosition, aimDirection, Color.red, grappleLayerMask);

            if (hit.collider != null)
            {
                grappleAttached = true;
                if (!grapplePositions.Contains(hit.point))
                {
                    grapplePositions.Add(hit.point);
                    joint.distance = Vector2.Distance(playerPosition, hit.point);
                    joint.enabled = true;
                    hingeAnchorSprite.enabled = true;
                }
            }
            else
            {
                grappleRenderer.enabled = false;
                grappleAttached = false;
                joint.enabled = false;
            }
        }

        if (Input.GetMouseButton(1))
        {
            ResetRope();
        }

        //UpdateGrapplePositions();
    }

    private void ResetRope()
    {
        joint.enabled = false;
        grappleAttached = false;
        grappleRenderer.positionCount = 2;
        grappleRenderer.SetPosition(0, transform.position);
        grappleRenderer.SetPosition(1, transform.position);
        grapplePositions.Clear();
        hingeAnchorSprite.enabled = false;
    }

    private void UpdateGrapplePositions()
    {
        if (!grappleAttached)
            return;

        grappleRenderer.positionCount = grapplePositions.Count + 1;

        for (var i = grappleRenderer.positionCount - 1; i >= 0; i --)
        {
            if (i != grappleRenderer.positionCount - 1)
            {
                grappleRenderer.SetPosition(i, grapplePositions[i]);

                if (i == grapplePositions.Count - 1 || grapplePositions.Count == 1)
                {
                    var grapplePosition = grapplePositions[grapplePositions.Count - 1];
                    if (grapplePositions.Count == 1)
                    {
                        hingeAnchorRb.transform.position = grapplePosition;
                        if (!distanceSet)
                        {
                            joint.distance = Vector2.Distance(transform.position, grapplePosition);

                            distanceSet = true;
                        }
                    }
                    else
                    {
                        hingeAnchorRb.transform.position = grapplePosition;
                        if (!distanceSet)
                        {
                            joint.distance = Vector2.Distance(transform.position, grapplePosition);

                            distanceSet = true;
                        }
                    }
                }
                else if (i - 1 == grapplePositions.IndexOf(grapplePositions.Last()))
                {
                    var grapplePosition = grapplePositions.Last();
                    hingeAnchorRb.transform.position = grapplePosition;
                    if (!distanceSet)
                    {
                        joint.distance = Vector2.Distance(transform.position, grapplePosition);

                        distanceSet = true;
                    }
                }
            }
            else
            {
                grappleRenderer.SetPosition(i, transform.position);
            }
        }
    }
}