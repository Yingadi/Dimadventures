using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    Animator animator;

    void Awake() 
    {
        animator = GetComponent<Animator>();    
    }

    void Start() 
    {
        StartCoroutine(CorTurnOnTrapDelay());
    }

    IEnumerator CorTurnOnTrapDelay()
    {
        yield return new WaitForSeconds(Random.Range(4f, 6f));
        animator.SetBool("On", true);
        StartCoroutine(CorTurnOffTrapDelay());
    }

    IEnumerator CorTurnOffTrapDelay()
    {
        yield return new WaitForSeconds(Random.Range(3f, 4f));
        animator.SetBool("On", false);
        StartCoroutine(CorTurnOnTrapDelay());
    }
}
