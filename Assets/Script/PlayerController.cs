using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    float horizontal;
    float vertical;
    public float speed = 8f;
    public float jumpingPower = 16f;
    public float fastfallForce = 2f;
    public float jumpstallForce = 10f;
    public float tapjumpModifier = 0.5f;
    public float stallCooldown = 1f;
    bool isFacingRight = true;

    bool canStall = true;
    bool isStalling;

    bool canDash = true;
    bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] TrailRenderer trailRenderer;


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

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            Debug.Log("I'm jumping");
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
            Debug.Log("Im blocking");
        }

        if (Input.GetButtonUp("Fire2"))
        {
            Debug.Log("Im countering");
        }

        Flip();
    }

    private void FixedUpdate()
    {
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


}
