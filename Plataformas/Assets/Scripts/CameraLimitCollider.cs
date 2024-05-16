using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimitCollider : MonoBehaviour
{
    [SerializeField] bool isLeftCollider;
    [SerializeField] BoxCollider2D otherCollider;
    [SerializeField] float leftNewMinLimitX, leftNewMinLimitY, leftNewMaxLimitX, leftNewMaxLimitY;

    [SerializeField] float rightNewMinLimitX, rightNewMinLimitY, rightNewMaxLimitX, rightNewMaxLimitY;

    CameraFollow cf;

    private void Awake() 
    {
        cf = GameObject.Find("CameraManager").GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isLeftCollider)
            {
                Debug.Log("Entro por la izquierda");
                GetComponent<BoxCollider2D>().enabled = false;
                otherCollider.enabled = true;

                cf.ChangeLimits(leftNewMinLimitX, leftNewMinLimitY, leftNewMaxLimitX, leftNewMaxLimitY);
            }
            else
            {
                Debug.Log("Entro por la derecha");
                GetComponent<BoxCollider2D>().enabled = false;
                otherCollider.enabled = true;

                cf.ChangeLimits(rightNewMinLimitX, rightNewMinLimitY, rightNewMaxLimitX, rightNewMaxLimitY);
            }
        }    
    }
}
