using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    
    float horizontal;
    float vertical;

    [Header("Player Variables")]
    public int maxHealth = 100;
    public int healthBonus = 0;
    public int healing;
    public int currentHealth;

    [Header("Movement Variables")]
    public float speed = 8f;
    bool isFacingRight = true;

    [Header("Stalling Variables")]
    public float stallCooldown = 0.5f;
    public float jumpstallForce = 500f;
    bool canStall = true;
    bool isStalling;

    [Header("Jump Variables")]
    public float jumpingPower = 20f;
    public float doubleJumpingPower = 16f;
    public float fastfallForce = 1000f;
    public float tapjumpModifier = 0.5f;
    bool doubleJump;

    [Header("Dash Variables")]
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    bool isDashing;
    bool canDash = true;

    [Header("Counter Variables")]
    public float counterRange = 0.5f;
    public GameObject punch;
    //bool isCountering = false;
    //bool canCounter = true;
    //public float counterStartUp = 0.3f;

    [Header("Block Variables")]
    public float blockRange = 1f;
    //bool isBlocking = false;
    //bool canBlock = true;

    [Header("Wall Sliding Variables")]
    public float wallSlidingSpeed = 2f;
    bool isWallSliding;

    [Header("Wall Jumping Variables")]
    public float wallJumpingTime = 2f;
    public float wallJumpingDuration = 0.4f;
    bool isWallJumping;
    float wallJumpingDirection;
    float wallJumpingCounter;
    Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] Transform counterPoint;
    [SerializeField] Transform blockPoint;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;

    Animator anim;


    private void Start()
    {
        currentHealth = maxHealth + healthBonus;

        anim = GetComponent<Animator>();

        _UI.SetMaxHealth(currentHealth);
        
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

                anim.SetTrigger("Jump");
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * tapjumpModifier);
            Debug.Log("I'm tap jumping");

            anim.SetTrigger("Jump");
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
            anim.SetTrigger("Fall");
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
            Block();
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Baguette"))
        {
            TakeDamage(10);
            Debug.Log("Im hit, i have" + currentHealth + " health left");
            
        }
    }

    public void Heal(int _heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += _heal;
            _UI.UpdateHealthBar(currentHealth);
        }
        else
        {
            _GM.AddScore(1);
        }
    }


    /// <summary>
    /// allows the player to take damage
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        _UI.UpdateHealthBar(currentHealth);
        _UI.ResetBlockCounter();
        if (currentHealth < 0)
        {
            Die();
        }
        
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    void Die()
    {
        Debug.Log("im ded yo!");
        Destroy(this.gameObject);
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


    /// <summary>
    /// Check if the player is wallsliding
    /// </summary>
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

    /// <summary>
    /// check if the player can walljump
    /// </summary>
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

    /// <summary>
    /// stop walljumping
    /// </summary>
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
        anim.SetTrigger("Dash");
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
        anim.SetTrigger("Stall");
        yield return new WaitForSeconds(stallCooldown);
        isStalling = false;
        yield return new WaitForSeconds(stallCooldown);
        canStall = true;
    }

    //IEnumerator Counter()
    //{
    //    isCountering = true;
    //    canCounter = false;
    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(counterPoint.position, counterRange, enemyLayers);
    //    yield return new WaitForSeconds(2);
    //    isCountering = false;
    //    yield return new WaitForSeconds(stallCooldown);
    //    canCounter = true;
    //}

    /// <summary>
    /// The ability to create a small hitbox infront of the player to counter-attack an enemy or projectile
    /// </summary>
    void Counter()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(counterPoint.position, counterRange, enemyLayers);

        anim.SetTrigger("Counter");
        //punch.SetActive(true);
        //Destroy the enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Bread>().Destroy();
            Debug.Log("We Hit " + enemy.name);
            _GM.OnBreadCountered();
            Idle();
            //Destroy(enemy);
        }

        
        //yield return new WaitForSeconds(1);
        //punch.SetActive(false);
    }

    /// <summary>
    /// ability to create a shiled that surround the player protecting them for a brief moment
    /// </summary>
    void Block()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(blockPoint.position, blockRange, enemyLayers);

        anim.SetTrigger("Block");
        //punch.SetActive(true);
        //Destroy the enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Bread>().Destroy();
            Debug.Log("We block " + enemy.name);
            _GM.OnBreadBlocked();
            //Destroy(enemy);
        }

        
        //yield return new WaitForSeconds(1);
        //punch.SetActive(false);
    }

    void Idle()
    {
        anim.SetTrigger("Idle");
    }

    /// <summary>
    /// Show hitbox in editor
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (counterPoint && blockPoint == null)
            return;

        Gizmos.DrawWireSphere(blockPoint.position, blockRange);
        
        Gizmos.DrawWireSphere(counterPoint.position, counterRange);
    }





}
