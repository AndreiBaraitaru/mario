using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverGoomba : MonoBehaviour
{
    public Transform enemyLocation;
    private bool onGroundState;
    private bool countScoreState = false;

    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown("space") && onGroundCheck())
        {
            onGroundState = false;
            countScoreState = true;
        }

        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            onGroundState = true;
    }

    private bool onGroundCheck()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
}
