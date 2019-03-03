using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameOverScreen gameOverScreen;
    
    private int score = 0;
    private bool isGamePlaing = false;
    private float timeLeft;
    [SerializeField] private float gameDuration = 100f;
  

    // Start is called before the first frame update
    void Start()
    {
         
        StartGame();
    }

    private void StartGame()
    {
        Time.timeScale = 1;
        score = 0;
        UpdateScoreUI();
        timeLeft = gameDuration;
        isGamePlaing = true;
    }

    public void OnRestart()
    {
//        StartGame(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OnAgentFlewAway(FlyingAgent flyingAgent)
    {
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {score}";
    }

    private void FixedUpdate()
    {
        if (isGamePlaing)
        {
            countdownText.text = $"Time left: {(int) timeLeft}";
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        isGamePlaing = false;
        gameOverScreen.Show(score);
    }
}
