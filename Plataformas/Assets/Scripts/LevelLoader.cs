using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    
    [SerializeField] Animator transitionAnimator;
    [SerializeField] float transitionTime = 1f;

    public void LoadNextLevel(int levelIndex)
    {
        StartCoroutine(CorLoadLevel(levelIndex));
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(CorLoadLevel(sceneName));
    }

    IEnumerator CorLoadLevel(int levelIndex) {
        {
            transitionAnimator.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(levelIndex);
        }
    }

    IEnumerator CorLoadLevel(string sceneName) {
        {
            transitionAnimator.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(sceneName);
        }
    }
}
