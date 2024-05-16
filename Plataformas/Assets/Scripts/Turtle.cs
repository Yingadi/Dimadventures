using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    [SerializeField] LayerMask playerMask;
    [SerializeField] float minXMovement, maxXMovement, speed;

    Animator animator;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Vector2 boxSize;
    float boxCastDistance;
    [SerializeField] bool isIdleSpikesIn = true;
    //bool isIdleSpikesOut = false;
    [SerializeField] bool turtleHitted = false, turtleAlive = true;
    float xDirection = 1;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        GetComponent<SpriteRenderer>().flipX = true;
    }

    void Start()
    {
        StartCoroutine(CorSpikesOutDelay());

        boxSize = new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y - 0.2f);
        boxCastDistance = 0.1f;
    }

    void FixedUpdate()
    {
        if (turtleHitted)
        {
            if (GetComponent<SpriteRenderer>().flipX == true)
            {
                rb.AddForce(new Vector2(2, 3), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-2, 3), ForceMode2D.Impulse);
            }

            Debug.Log("desde arriba");
            turtleHitted = false;
        }

        rb.velocity = new Vector2(speed * xDirection, 0);

        if (transform.localPosition.x >= maxXMovement)
        {
            xDirection = -1;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (transform.localPosition.x <= minXMovement)
        {
            xDirection = 1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    IEnumerator CorSpikesOutDelay()
    {
        yield return new WaitForSeconds(Random.Range(3f, 4.3f));
        //sacar pinchos
        animator.SetTrigger("SpikesOut");
        //yield return new WaitForSeconds(0.55f);
        isIdleSpikesIn = false;
        //isIdleSpikesOut = true;
        StartCoroutine(CorSpikesInDelay());
    }

    IEnumerator CorSpikesInDelay()
    {
        yield return new WaitForSeconds(Random.Range(3f, 4.3f));
        //guardar pinchos
        animator.SetTrigger("SpikesIn");
        yield return new WaitForSeconds(0.55f);
        isIdleSpikesIn = true;
        //isIdleSpikesOut = false;
        StartCoroutine(CorSpikesOutDelay());
    }

    bool IsHittedFromAbove()
    {
        Vector2 boxCastOrigin = new Vector2(transform.position.x, transform.position.y + boxSize.y / 2);
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxSize, 0f, Vector2.up, boxCastDistance, playerMask);
        return hit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        //if (other.gameObject.tag.Equals("Player"))
        //{
            if (isIdleSpikesIn && IsHittedFromAbove() && turtleAlive && other.gameObject.CompareTag("Player"))
            {
                //se hace hit -> pequeña fuerza para atrás y arriba
                GetComponent<BoxCollider2D>().enabled = false;
                turtleHitted = true;
                turtleAlive = false;
                other.gameObject.GetComponent<Player2>().SetEnemyHitted();
                animator.SetTrigger("Hit");
                maxXMovement += 10;
                minXMovement -= 10;
                StartCoroutine(CorDeath());
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                //muere player
                other.gameObject.GetComponent<Player2>().PlayerDeath();
            }
        //}
    }

    IEnumerator CorDeath()
    {
        if (GetComponent<SpriteRenderer>().flipX == true)
        {
            for (int i = 0; i < 36; i++)
            {
                yield return new WaitForSeconds(0.01f);
                transform.Rotate(new Vector3(0, 0, -5));
            }
        }
        else
        {
            for (int i = 0; i < 36; i++)
            {
                yield return new WaitForSeconds(0.01f);
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
    }

    public void TurtleDeath()
    {
        Destroy(gameObject);
    }
}
