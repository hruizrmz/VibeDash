using UnityEngine;

public class Runner : MonoBehaviour
{
    public bool jumpInput;

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
        // anim.SetBool("isIdle", true);
    }

    void Update()
    {
        if (isGrounded())
        {
            if (jumpInput)
            {
                anim.SetBool("isJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpVel);
            } else
            {
                anim.SetBool("isFalling", false);
            }
        } else
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseMod);
            }
            else
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", true);
                jumpInput = false;
            }
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
