using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Mendeklarasikan tipe data dan variabel
    [Header("Game Settings")]
    public int player1Score;
    public int player2Score;
    public float timer;
    public bool isOver;
    public bool goldenGoal;
    public bool isSpawnPowerUp;
    public GameObject ballSpawned;

    [Header("prefab")]
    public GameObject BallPrefab;
    public GameObject[] powerUp; // bentul array

    [Header("Panels")]
    public GameObject PausePanel;
    public GameObject GameOverPanel;

    [Header("InGame UI")]
    public TextMeshProUGUI timertxt;
    public TextMeshProUGUI player1ScoreTxt;
    public TextMeshProUGUI player2ScoreTxt;
    public GameObject goldenGoalUI;

    [Header("Game Over UI")]
    public GameObject player1WinUI;
    public GameObject player2WinUI;
    public GameObject youWin;
    public GameObject youLose;



    private void Awake() // memvalidasi
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        GameOverPanel.SetActive(false);

        player1WinUI.SetActive(false);
        player2WinUI.SetActive(false);
        youWin.SetActive(false);
        youLose.SetActive(false);

        youLose.SetActive(false);
        goldenGoalUI.SetActive(false);

        timer = GameData.instance.gameTimer;
        isOver = false;
        goldenGoal = false;

        SpawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        player1ScoreTxt.text = player1Score.ToString();
        player2ScoreTxt.text = player2Score.ToString();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60); //modulus
            timertxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (seconds % 20 == 0 && !isSpawnPowerUp)
            {
                StartCoroutine("SpawnPowerUp");
            }
            
        }
        if (timer <= 0f && !isOver)
        {
            timertxt.text = "00:00";
            if (player1Score == player2Score)
            {
                if (!goldenGoal)
                {
                    goldenGoal = true;

                    Ball[] ball = FindObjectsOfType<Ball>(); // Fungsi supaya dalam mode golden ball tidak double
                    for (int i = 0; i < ball.Length; i++)
                    {
                        Destroy(ball[i].gameObject);
                    }

                    goldenGoalUI.SetActive(true);

                    SpawnBall();
                }
            }
            else
            {
                GameOver();
            }
        }
    }

    public IEnumerator SpawnPowerUp()
    {
        isSpawnPowerUp = true;
        Debug.Log("Power Up");
        int rand = Random.Range(0, powerUp.Length);
        Instantiate(powerUp[rand], new Vector3(Random.Range(-3.2f, 3.2f), Random.Range(-2.35f, 2.35f), 0), Quaternion.identity);
        yield return new WaitForSeconds(1);
        isSpawnPowerUp = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        SoundManager.instance.UIClickSfx();
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        SoundManager.instance.UIClickSfx();
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("1. Main Menu");
        SoundManager.instance.UIClickSfx();
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("2. Gameplay");
        SoundManager.instance.UIClickSfx();
    }

    public void SpawnBall()
    {
        Debug.Log("Muncul Bola");
        StartCoroutine("DelaySpawn");
    }

    public void GameOver()
    {
        SoundManager.instance.UIClickSfx();
        isOver = true;
        Debug.Log("Game Over");
        Time.timeScale = 0;

        GameOverPanel.SetActive(true);

        if (!GameData.instance.isSinglePlayer) //Multiplayer
        {
            if (player1Score > player2Score)
            {
                player1WinUI.SetActive(true);
            }
            if (player1Score < player2Score)
            {
                player2WinUI.SetActive(true);
            }
        }
        else
        {
            if (player1Score > player2Score) //single player
            {
                youWin.SetActive(true);
            }
            if (player1Score < player2Score)
            {
                youLose.SetActive(true);
            }
        }
    }

    private IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(3);
        if (ballSpawned == null)
        {
            ballSpawned = Instantiate(BallPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
