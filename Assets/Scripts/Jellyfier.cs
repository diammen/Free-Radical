using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfier : MonoBehaviour
{

    public float bounceSpeed;
    public float fallForce;
    public float stiffness;

    private MeshFilter meshFilter;
    private Mesh mesh;

    JellyVertex[] jellyVerts;
    Vector3[] currentMeshvertices;

    // Use this for initialization
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        GetVertices();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < jellyVerts.Length; i++)
        {
            jellyVerts[i].UpdateVelocity(bounceSpeed);
            jellyVerts[i].Settle(stiffness);

            jellyVerts[i].currentVertexPosition += jellyVerts[i].currentVelocity * Time.deltaTime;
            currentMeshvertices[i] = jellyVerts[i].currentVertexPosition;
        }

        mesh.vertices = currentMeshvertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    private void GetVertices()
    {
        jellyVerts = new JellyVertex[mesh.vertices.Length];
        currentMeshvertices = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            jellyVerts[i] = new JellyVertex(i, mesh.vertices[i], mesh.vertices[i], Vector3.zero);
            currentMeshvertices[i] = mesh.vertices[i];
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] collisionPoints = collision.contacts;
        for (int i = 0; i < collisionPoints.Length; i++)
        {
            Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * 0.1f);
            
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            Vector3 inputPoint = contactPoints[i].point + (contactPoints[i].point * 0.1f);
            ApplyPressureToPoint(inputPoint, fallForce);
        }
    }
    public void ApplyPressureToPoint(Vector3 _point, float _pressure)
    {
        for (int i = 0; i < jellyVerts.Length; i++)
        {
            jellyVerts[i].ApplyPressureToVertex(transform, _point, _pressure);
        }
    }
}