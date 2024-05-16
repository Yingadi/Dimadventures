using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<Player2>().PlayerDeath();
        }    
    }
}
