using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtWin, txtIns1, txtIns2;
    Animator txtDeathAnimator, txtIns1Animator, txtIns2Animator;

    bool interactable = false;

    void Awake()
    {
        txtDeathAnimator = txtWin.GetComponent<Animator>();
        txtIns1Animator = txtIns1.GetComponent<Animator>();
        txtIns2Animator = txtIns2.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(CorTextsDelay());
    }

    void Update()
    {
        if (interactable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //salir
                Debug.Log("Salir");
                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel("StartScene");
            }
        }
    }

    IEnumerator CorTextsDelay()
    {
        yield return new WaitForSeconds(1f);

        txtDeathAnimator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        txtIns1Animator.SetTrigger("FadeIn");
        txtIns2Animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(0.5f);

        interactable = true;
    }
}
