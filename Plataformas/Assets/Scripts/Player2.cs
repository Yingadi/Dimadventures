using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    //Serialized
    [SerializeField] float speed = 1;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpForce;
    [SerializeField] AudioClip jumpClip, deathClip;

    //Private
    Rigidbody2D rb;
    float movement;
    bool facingRight = true;
    bool wannaJump = false;
    bool wannaDoubleJump = false;
    bool doubleJumped = false;
    bool wallJumped = false;
    bool isOnWall = false;
    Animator animator;
    BoxCollider2D boxCollider;
    float colliderToBoxCastDif = .01f;
    float castDistance = .1f;
    GameObject lastWall;
    bool enemyHitted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(CorStartAnim());
        animator.SetTrigger("StartAnim");
    }

    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded())
            {
                wannaJump = true;
            }
            else if (!isGrounded())
            {
                wannaDoubleJump = true;
            }
        }

        if (rb.velocity.y < 0)
        {
            animator.SetBool("Falling", true);
            animator.SetBool("Jumping", false);
            animator.SetBool("DoubleJumping", false);
        }
        else if (rb.velocity.y > 0)
        {
            animator.SetBool("Falling", false);
        }

        if (!isGrounded() && isNextToTheWall())
        {
            isOnWall = true;
            animator.SetBool("Falling", false);
            animator.SetBool("Jumping", false);
        }
        else
        {
            isOnWall = false;
        }

        if (isOnWall && !wallJumped)
        {
            doubleJumped = false;
            wallJumped = true;
        }

        animator.SetBool("Grounded", isGrounded());
        animator.SetBool("OnWall", isOnWall);

        if (isGrounded())
        {
            doubleJumped = false;
            wallJumped = false;
            animator.SetBool("Falling", false);
        }
    }

    private void FixedUpdate()
    {
        Move(movement);

        if (wannaJump)
        {
            Jump();
        }

        if (wannaDoubleJump)
        {
            DoubleJump();
        }

        if (enemyHitted)
        {
            EnemyHit();
        }
    }

    void Move(float movementValue)
    {
        Vector2 targetVelocity = new Vector2(movementValue * speed * 100 * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = targetVelocity;

        if (facingRight && movementValue < 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            GetComponent<SpriteRenderer>().flipX = true;
            facingRight = false;
        }
        else if (!facingRight && movementValue > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            GetComponent<SpriteRenderer>().flipX = false;
            facingRight = true;
        }

        if (rb.velocity.x != 0)
        {
            animator.SetFloat("xVelocity", 0.5f);
        }
        else if (rb.velocity.x == 0)
        {
            animator.SetFloat("xVelocity", 0f);
        }
    }

    void Jump()
    {
        wannaJump = false;

        animator.SetBool("Jumping", true);

        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            AudioManager.Instance.PlayClip(jumpClip);
        }
    }

    void DoubleJump()
    {
        wannaDoubleJump = false;

        animator.SetBool("DoubleJumping", true);
        animator.SetBool("Jumping", false);

        if (!doubleJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, (jumpForce - 50) * Time.fixedDeltaTime);
            doubleJumped = true;
            AudioManager.Instance.PlayClip(jumpClip);
        }
    }

    bool isGrounded()
    {
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x - colliderToBoxCastDif, boxCollider.bounds.size.y - colliderToBoxCastDif);
        var boxCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxSize, 0f, Vector2.down, castDistance, groundLayer);
        return boxCastHit.collider != null;
    }

    bool isNextToTheWall()
    {
        Vector2 directionToTest = facingRight ? Vector2.right : Vector2.left;
        var BoxCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, directionToTest, 0.1f, groundLayer);
        return BoxCastHit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOnWall)
        {
            if (lastWall != collision.gameObject)
            {
                wallJumped = false;
            }

            lastWall = collision.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("trap"))
        {
            PlayerDeath();
        }
    }

    IEnumerator CorStartAnim()
    {
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.18f);
        rb.gravityScale = 1;
    }

    void EnemyHit()
    {
        rb.AddForce(new Vector2(0, 7f), ForceMode2D.Impulse);
        enemyHitted = false;
    }

    public void SetEnemyHitted()
    {
        enemyHitted = true;
    }

    public void PlayerDeath()
    {
        Destroy(gameObject);
        AudioManager.Instance.PlayClip(deathClip);
        GameObject.Find("GameManager").GetComponent<GameManager>().SetDeathState();
    }
}
