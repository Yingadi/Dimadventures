using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEnemy : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb;
    Vector3 startPos;
    bool wannaAttack = false, attackCompleted = false, readyToAttack = true, groundReached = false;

    [SerializeField] float minAttackWait = 2, maxAttackWait = 5;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPos = transform.localPosition;
        StartCoroutine(CorBlink());
    }

    void Update()
    {
        if (readyToAttack)
        {
            StartCoroutine(CorAttackDelay());
            readyToAttack = false;
        }
    }

    void FixedUpdate()
    {
        if (wannaAttack)
        {
            rb.velocity = new Vector2(0, -700 * Time.fixedDeltaTime);
            wannaAttack = false;
        }

        if (attackCompleted)
        {
            rb.velocity = new Vector2(0, 200 * Time.fixedDeltaTime);

            if (transform.localPosition.y >= startPos.y)
            {
                attackCompleted = false;
                rb.velocity = new Vector2(0, 0);
                readyToAttack = true;
            }
        }

        if (groundReached)
        {
            rb.velocity = new Vector2(0, 0);
            groundReached = false;
        }
    }

    IEnumerator CorBlink()
    {
        yield return new WaitForSeconds(Random.Range(4f, 6f));
        animator.SetTrigger("Blink");
        StartCoroutine(CorBlink());
    }

    IEnumerator CorGoUpDelay()
    {
        yield return new WaitForSeconds(1f);
        attackCompleted = true;
    }

    IEnumerator CorAttackDelay()
    {
        yield return new WaitForSeconds(Random.Range(minAttackWait, maxAttackWait));
        wannaAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Ground"))
        {
            groundReached = true;
            animator.SetTrigger("BottomHit");
            StartCoroutine(CorGoUpDelay());
        }

        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<Player2>().PlayerDeath();
        }
    }
}
