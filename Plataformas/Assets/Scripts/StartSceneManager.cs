using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{

    [SerializeField] GameObject fadeImage, titleTextsCanvas;
    [SerializeField] Animator canvasAnimator;
    [SerializeField] AudioClip uiClick, startTheme;
    [SerializeField] Button btnContinue;

    void Start() 
    {
        AudioManager.Instance.SetTheme(startTheme);

        if (LevelData.Instance.GetCurrentLevel() != 0)
        {
            btnContinue.interactable = true;
        }
    }

    public void OnContinueClick()
    {
        GetComponent<AudioSource>().PlayOneShot(uiClick);
        Debug.Log("Se ha presionado Continue");
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel(LevelData.Instance.GetCurrentLevel());
    }

    public void OnStartClick()
    {
        GetComponent<AudioSource>().PlayOneShot(uiClick);
        Debug.Log("Se ha presionado Start");
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel("Level 1");
        canvasAnimator.SetTrigger("Fade");
    }

    public void OnExitClick()
    {
        GetComponent<AudioSource>().PlayOneShot(uiClick);
        Debug.Log("Se ha presionado Exit");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void DisableTitleTextsCanvas()
    {
        titleTextsCanvas.SetActive(false);
    }
}