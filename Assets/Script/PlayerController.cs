using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    float horizontal;
    float vertical;
    public float speed = 8f;
    public float jumpingPower = 16f;
    public float doubleJumpingPower = 8f;
    public float fastfallForce = 2f;
    public float jumpstallForce = 10f;
    public float tapjumpModifier = 0.5f;
    public float stallCooldown = 1f;
    bool isFacingRight = true;

    bool canStall = true;
    bool isStalling;

    bool doubleJump;

    bool canDash = true;
    bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;


    public float counterRange = 0.5f;

    public GameObject punch;

    [Header("Wall Sliding Variables")]
    bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    [Header("Wall Jumping Variables")]
    bool isWallJumping;
    float wallJumpingDirection;
    public float wallJumpingTime = 2f;
    float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] Transform counterPoint;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;


    private void Start()
    {
        //punch.SetActive(false);
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (isStalling)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (IsGrounded() && !Input.GetButtonDown("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);
                Debug.Log("I'm jumping");

                doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * tapjumpModifier);
            Debug.Log("I'm tap jumping");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
            Debug.Log("I'm dashing");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(Vector2.down * fastfallForce);
            Debug.Log("I'm fastfalling");
        }
        if (Input.GetKeyDown(KeyCode.W) && canStall)
        {
            StartCoroutine(Stall());
            Debug.Log("I'm stalling");
        }

        if (Input.GetButtonUp("Fire1"))
        {
            Counter();
            Debug.Log("Im countering");
        }

        if (Input.GetButtonUp("Fire2"))
        {
            Debug.Log("Im blocking");
        }

        WallSlide();

        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        
        
        if (isDashing)
        {
            return;
        }
        
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    /// <summary>
    /// Check if player is grounded
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    /// <summary>
    /// Check if player is touching a wall
    /// </summary>
    /// <returns></returns>
    bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    /// <summary>
    /// Flip the player sprite depending on where they are moving
    /// </summary>
    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// The ability to dash forward, while dashing, cannot do any other action
    /// </summary>
    /// <returns></returns>
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    /// <summary>
    /// The ability to micro jump after any action, but freezing the control for 0.5 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator Stall()
    {
        isStalling = true;
        canStall = false;
        rb.AddForce(Vector2.up * jumpstallForce);
        yield return new WaitForSeconds(stallCooldown);
        isStalling = false;
        yield return new WaitForSeconds(stallCooldown);
        canStall = true;
    }

    /// <summary>
    /// The ability to create a small hitbox infront of the player to counter-attack an enemy or projectile
    /// </summary>
    void Counter()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(counterPoint.position, counterRange, enemyLayers);
        //punch.SetActive(true);
        //Destroy the enemy
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We Hit " + enemy.name);
            //Destroy(enemy);
        }
        //yield return new WaitForSeconds(1);
        //punch.SetActive(false);
    }

    /// <summary>
    /// Show hitbox in ediotr
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (counterPoint == null)
            return;
        
        Gizmos.DrawWireSphere(counterPoint.position, counterRange);
    }

}