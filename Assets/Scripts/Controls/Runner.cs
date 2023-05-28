using UnityEngine;

public class Runner : MonoBehaviour
{
    private bool jumpInput;
    [SerializeField] private float jumpVel = 8f;
    [SerializeField] private float jumpReleaseMod = 1f;
    [SerializeField] private float maxGroundDistance = 1.1f;
    
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private LayerMask groundMask;
    private float feetPosition;
    private Ground ground;

    [SerializeField] private float outOfBounds;

    private Rigidbody2D rb;
    private Animator anim;

    #region Events
    private void OnEnable()
    {
        GameManager.StartGame += StartRunner;
        NoteObject.JumpNote += PlayerJump;
    }
    private void OnDisable()
    {
        GameManager.StartGame -= StartRunner;
        NoteObject.JumpNote -= PlayerJump;
    }
    private void StartRunner()
    {
        anim.SetBool("isRunning", true);
    }
    private void PlayerJump()
    {
        jumpInput = true;
    }

    #endregion

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent <Animator>();
        anim.SetBool("isRunning", false);
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
