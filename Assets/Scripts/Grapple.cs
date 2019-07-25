using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public GameObject hingeAnchor;
    public DistanceJoint2D joint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public Player player;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D hingeAnchorRb;
    private SpriteRenderer hingeAnchorSprite;

    public LineRenderer grappleRenderer;
    public LayerMask grappleLayerMask;
    private float grappleMaxCastDistance = 20f;
    private List<Vector2> grapplePositions = new List<Vector2>();

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
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Vector3 direction = worldMousePosition - transform.position;
        float aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle) * Vector2.right;
        playerPosition = transform.position;

        if (!ropeAttached)
        {
            SetCrosshairPosition(aimAngle);
        }
        else
        {
            crosshairSprite.enabled = false;
        }
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
}