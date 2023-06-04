using UnityEngine;

public class Runner : MonoBehaviour
{
    private bool isGameRunning;

    private bool jumpInput, longJumpInput, punchInput, slideInput;
    private float jumpVel = 8f;
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
        ScoreManager.StartGame += StartRunner;
        NoteObject.HoldNote += PlayerLongJump;
        NoteObject.SwipeUpNote += PlayerJump;
        NoteObject.SwipeDownNote += PlayerSlide;
        NoteObject.SwipeRightNote += PlayerPunch;
    }
    private void OnDisable()
    {
        ScoreManager.StartGame -= StartRunner;
        NoteObject.HoldNote -= PlayerLongJump;
        NoteObject.SwipeUpNote -= PlayerJump;
        NoteObject.SwipeDownNote -= PlayerSlide;
        NoteObject.SwipeRightNote -= PlayerPunch;
    }
    private void StartRunner()
    {
        isGameRunning = true;
    }
    private void PlayerJump()
    {
        jumpInput = true;
    }
    private void PlayerLongJump()
    {
        longJumpInput = true;
    }
    private void PlayerSlide()
    {
        slideInput = true;
    }
    private void PlayerPunch()
    {
        punchInput = true;
    }
    #endregion

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent <Animator>();
        isGameRunning = false;
        jumpInput = longJumpInput = slideInput = punchInput = false;
    }

    void Update()
    {
        if (transform.position.y <= outOfBounds) Destroy(this.gameObject);

        if (isGrounded())
        {
            if (feetPosition > ground.groundHeight)
            {
                if (!anim.GetBool("isJumping")) 
                {
                    if (jumpInput) // grounded, above height, and has not jumped yet = jumping
                    {
                        anim.SetBool("isJumping", true);
                        GetComponent<Rigidbody2D>().gravityScale = 2.5f;
                        jumpVel = 8f;
                        rb.velocity = new Vector2(rb.velocity.x, jumpVel);
                        jumpInput = false;
                    }
                    else if (longJumpInput)
                    {
                        anim.SetBool("isJumping", true);
                        GetComponent<Rigidbody2D>().gravityScale = 2f;
                        jumpVel = 10f;
                        rb.velocity = new Vector2(rb.velocity.x, jumpVel);
                        longJumpInput = false;
                    }
                    else if (slideInput)
                    {
                        anim.SetTrigger("isSliding");
                        slideInput = false;
                    }
                    else if (punchInput)
                    {
                        anim.SetTrigger("isPunching");
                        punchInput = false;
                    }
                    else if (isGameRunning && !anim.GetBool("isRunning"))
                    {
                        anim.SetBool("isRunning", true);
                    }
                }
                anim.SetBool("isFalling", false);
            }
            else // grounded, below height = falling off building
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", true);
            }
        }
        else
        {
            if (rb.velocity.y > 0) // not grounded, moving = still jumping
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseMod);
            }
            else // not grounded, not moving = falling after jumping
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", true);
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
