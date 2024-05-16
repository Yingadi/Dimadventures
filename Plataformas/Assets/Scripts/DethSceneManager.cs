using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DethSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtDeath, txtIns1, txtIns2;
    Animator txtDeathAnimator, txtIns1Animator, txtIns2Animator;

    [SerializeField] AudioClip deathTheme;

    bool interactable = false;

    void Awake() 
    {
        txtDeathAnimator = txtDeath.GetComponent<Animator>();
        txtIns1Animator = txtIns1.GetComponent<Animator>();   
        txtIns2Animator = txtIns2.GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(CorTextsDelay());
        AudioManager.Instance.StopTheme();
        GetComponent<AudioSource>().PlayOneShot(deathTheme);
    }

    void Update()
    {
        if (interactable)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //reintentar
                Debug.Log("Reintentar");
                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel(LevelData.Instance.GetCurrentLevel());
            }
            else if (Input.GetKeyDown(KeyCode.F))
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
