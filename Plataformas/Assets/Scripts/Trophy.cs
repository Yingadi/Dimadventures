using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{

    [SerializeField] AudioClip trophyClip;
    bool trophyLifted = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(LiftTrophy());
            GameObject.Find("GameManager").GetComponent<GameManager>().SetWinState();
        }
    }

    IEnumerator LiftTrophy()
    {

        if (!trophyLifted)
        {
            GetComponent<AudioSource>().PlayOneShot(trophyClip);
            for (int i = 0; i < 45; i++)
            {
                yield return new WaitForSeconds(0.004f);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.05f, transform.localPosition.z);
            }

            trophyLifted = true;
        }
    }
}
