using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CirSector : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer spriteRenderer;

    public float angleRange = 45f;
    public float distance = 5f;
    public float longDistance = 7.5f;
    public bool isCollisionNear = false;
    public bool isCollisionFar = false;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    Color _green = new Color(0, 1, 0, 0.2f);
    Color _cyan = new Color(0, 1, 1, 0.2f);

    Vector3 direction;

    float dotValue = 0f;

    void Update()
    {
        Vector3 transformValue = spriteRenderer.flipX ? transform.right * -1 : transform.right;
        dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
        direction = target.position - transform.position;

        if (direction.magnitude < longDistance)
            isCollisionFar = Vector3.Dot(direction.normalized, transformValue) > dotValue;
        else isCollisionFar = false;

        if (direction.magnitude < distance)
            isCollisionNear = Vector3.Dot(direction.normalized, transformValue) > dotValue;
        else isCollisionNear = false;



    }

    private void OnDrawGizmos()
    {
        Vector3 transformValue = spriteRenderer.flipX ? transform.right * -1 : transform.right;

        Handles.color = isCollisionFar ? _green : _cyan;
        Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, angleRange / 2, longDistance);
        Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, -angleRange / 2, longDistance);

        Handles.color = isCollisionNear ? _red : _blue;
        Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, angleRange / 2, distance);
        Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, -angleRange / 2, distance);


    }
}