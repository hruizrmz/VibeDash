using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerMovement : MonoBehaviour
{
    public float jumpVel = 10f;
    public float jumpReleaseMod = 2f;
    public float maxGroundDistance = 0.05f;
    public Vector3 boxSize;
    public LayerMask groundMask;

    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent <Animator>();
    }

    void Update()
    {
        var jumpInput = Input.GetKeyDown(KeyCode.Space);
        var jumpInputReleased = Input.GetKeyUp(KeyCode.Space);

        if (jumpInput && isGrounded())
        {
            // rb.AddForce(transform.up * jumpFactor, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpVel);
        }

        if (jumpInputReleased && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseMod);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxGroundDistance, boxSize);
    }

    private bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxGroundDistance, groundMask))
        {
            return true;
        } else
        {
            return false;
        }
    }
}
