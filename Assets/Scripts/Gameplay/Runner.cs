using UnityEngine;

public class Runner : MonoBehaviour
{
    public bool jumpInput;
    public float jumpVel = 10f;
    public float jumpReleaseMod = 2f;

    public float maxGroundDistance = 0.05f;
    private float feetPosition;
    public Vector3 boxSize;

    public float outOfBounds;

    public LayerMask groundMask;
    private Ground ground;

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
        if (transform.position.y <= outOfBounds) Destroy(this.gameObject);

        if (isGrounded())
        {
            if (feetPosition > ground.groundHeight)
            {
                if (jumpInput && !anim.GetBool("isJumping")) // grounded, above height, and has not jumped yet = jumping
                {
                    anim.SetBool("isJumping", true);
                    rb.velocity = new Vector2(rb.velocity.x, jumpVel);
                }
                else // grounded, above height, and did not jump = landed/running
                {
                    anim.SetBool("isFalling", false);
                }
            }
            else // grounded, below height = falling off building
            {
                anim.SetBool("isFalling", true);
            }
        } else
        {
            if (rb.velocity.y > 0) // not grounded, moving = still jumping
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseMod);
            }
            else // not grounded, not moving = falling after jumping
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
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxGroundDistance, groundMask);
        feetPosition = hit.point.y;

        if (hit.collider != null)
        {
            ground = hit.collider.gameObject.GetComponent<Ground>();
            return true;
        } else
        {
            ground = null;
            return false;
        }
    }
}
