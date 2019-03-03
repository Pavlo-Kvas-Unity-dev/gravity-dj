using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameOverScreen gameOverScreen;
    
    private int score = 0;
    private bool isGamePlaing = false;
    private float timeLeft;
    [SerializeField] private float gameDuration = 100f;
    
    private int? highScore;

    // Start is called before the first frame update
    private int HighScore
    {
        get
        {
            TryUpdateHighScore();
            
            return highScore.Value;
        }
        set
        {
            highScore = value;
            SaveHighScore(value);
        }
    }

    [ContextMenu("ClearHighScore")]
    public void ClearHighScore()
    {
        HighScore = 0;
    }
    
    private void SaveHighScore(int value)
    {
        PlayerPrefs.SetInt(nameof(highScore), value);
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            TryUpdateHighScore();
        }
    }

    void Start()
    {
        StartGame();
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

    private void StartGame()
    {
        Time.timeScale = 1;
        Score = 0;
        UpdateScore();
        timeLeft = gameDuration;
        isGamePlaing = true;
        
        UpdateHighScoreUI();
    }

    private void UpdateHighScoreUI()
    {
        bestScoreText.text = $"Best score: {HighScore}";
    }

    private void TryUpdateHighScore()
    {
        ReadHighScoreIfNull();
        if (Score > highScore)
        {
            highScore = Score;
            SaveHighScore(highScore.Value);
        }
    }

    private void ReadHighScoreIfNull()
    {
        if (!highScore.HasValue)
        {
            highScore = PlayerPrefs.GetInt(nameof(highScore), 0);
        }
    }

    public void OnRestart()
    {
//        StartGame(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnAgentFlewAway(FlyingAgent flyingAgent)
    {
        Score++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = $"Score: {Score}";
    }

    private void GameOver()
    {
        
        Time.timeScale = 0;
        isGamePlaing = false;
        UpdateHighScoreUI();
        gameOverScreen.Show(Score, HighScore);
    }
}
