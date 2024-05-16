using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum GameStates
    {
        Start,
        Midgame,
        Pause,
        Death,
        Win
    }
    GameStates gameState;

    GameObject player;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] float spawnX, spawnY;

    bool isPlayerDeath = false, isGameWon = false, gamePaused = false;
    int currentLevel;

    [SerializeField] AudioClip lvl1Theme, lvl2Theme, pauseClip, quitPauseClip;
    [SerializeField] GameObject pauseCanvas;

    void Awake()
    {

        currentLevel = SceneManager.GetActiveScene().buildIndex;
        LevelData.Instance.SetCurrentLevel(currentLevel);

        switch (currentLevel)
        {
            case 1:
                AudioManager.Instance.SetTheme(lvl1Theme);
                break;

            case 2:
                AudioManager.Instance.SetTheme(lvl2Theme);
                break;
        }
    }

    void Update()
    {
        switch (gameState)
        {
            case GameStates.Start:
                StartUpdate();
                break;

            case GameStates.Midgame:
                MidgameUpdate();
                break;

            case GameStates.Pause:
                PauseUpdate();
                break;

            case GameStates.Death:
                DeathUpdate();
                break;

            case GameStates.Win:
                WinUpdate();
                break;
        }
    }

    void StartUpdate()
    {
        StartCoroutine(CorSpawnPlayer());
        gameState = GameStates.Midgame;
    }

    void MidgameUpdate()
    {

        if (!gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0f;
                gamePaused = true;
                //activar canvas de pausa
                pauseCanvas.SetActive(true);
                GetComponent<AudioSource>().PlayOneShot(pauseClip);
                gameState = GameStates.Pause;
            }
        }
    }

    void PauseUpdate()
    {
        if (gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnpauseGame();
            }
        }
    }

    void DeathUpdate()
    {
        if (!isPlayerDeath)
        {
            Debug.Log("Falleciste");
            isPlayerDeath = true;
            StartCoroutine(CorDeathAnim());
        }
    }

    void WinUpdate()
    {
        if (!isGameWon)
        {
            Debug.Log("Ganaste");
            isGameWon = true;

            StartCoroutine(CorWinAnim());
        }
    }

    IEnumerator CorSpawnPlayer()
    {
        yield return new WaitForSeconds(2f);

        player = Instantiate(playerPrefab, new Vector3(spawnX, spawnY, 0), Quaternion.identity);
    }

    public void SetDeathState()
    {
        gameState = GameStates.Death;
    }

    public void SetWinState()
    {
        gameState = GameStates.Win;
    }

    IEnumerator CorDeathAnim()
    {
        yield return new WaitForSeconds(0.6f);
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel("DeathScene");
    }

    IEnumerator CorWinAnim()
    {

        yield return new WaitForSeconds(2f);

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                Debug.Log("Te pasaste el juego");
                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel("WinScene");
                break;

            default:
                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        //desactivar canvas de pausa
        pauseCanvas.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(quitPauseClip);
        gameState = GameStates.Midgame;
    }

    public void OnResumeClick()
    {
        UnpauseGame();
    }

    public void OnExitClick()
    {
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel("StartScene");
    }
}
