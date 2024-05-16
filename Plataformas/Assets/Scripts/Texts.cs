using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Texts : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<TextMeshPro>().alpha = 0.0f;
    }

    IEnumerator CorTextAppear()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.05f);
            gameObject.GetComponent<TextMeshPro>().alpha += 0.05f;
        }

        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(CorTextAppear());
        }
    }
}
