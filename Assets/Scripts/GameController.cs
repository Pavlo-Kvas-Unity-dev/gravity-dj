using System;
using InfinityEngine.Localization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI countdownText;
    
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private Button mainMenuButton;
    
    private int score = 0;
    private bool isGamePlaing = false;
    private float timeLeft;
    [SerializeField] private float gameDuration = 100f;
    
    private int? highScore;

    [SerializeField] private HelpWindow helpWindow;
    [SerializeField] private MainMenuWindow mainMenuWindow;

    // Start is called before the first frame update

    private int HighScore
    {
        get
        {
            ReadHighScoreIfNull();
            
            return highScore.Value;
        }
        set
        {
            highScore = value;
            SaveHighScore(value);
            UpdateHighScoreUI();
        }
    }

    private static void SaveHighScore(int value)
    {
        PlayerPrefs.SetInt(nameof(highScore), value);
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            UpdateHighScore();
        }
    }

    void Awake()
    {
        ISILocalization.onLanguageChanged += OnLanguageChanged;
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnLanguageChanged()
    {
        UpdateScore();
        UpdateHighScoreUI();
    }

    void Start()
    {
        ShowMainMenu();
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

    private void UpdateHighScore()
    {
        if (HighScore < Score)
        {
            HighScore = Score;
        }
    }

    private void StartGame()
    {
        Score = 0;
        UpdateScore();
        timeLeft = gameDuration;
        Resume();
                
        UpdateHighScoreUI();
        
    }

    private void UpdateHighScoreUI()
    {
        bestScoreText.text = string.Format(R3.strings.BestScoreFormat, HighScore);
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

    public void OnMainMenuButtonClicked()
    {
        Pause();
        ShowMainMenu(true);
    }

    private void ShowMainMenu(bool isGamePlaying=false)
    {
        mainMenuWindow.Init(isGamePlaying);
        mainMenuWindow.Open(
             StartGame, 
            Resume, ()=> { helpWindow.Open(null); }, Application.Quit);
    }

    private void Pause() 
    {
        Time.timeScale = 0;
        isGamePlaing = false;
    }

    public void OnHelpWindowClosed()
    {
        Resume();
    }

    private void Resume()
    {
        Time.timeScale = 1;
        isGamePlaing = true;
    }

    public void OnAgentFlewAway(FlyingAgent flyingAgent)
    {
        Score++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = string.Format(R3.strings.ScoreFormat, Score);
    }

    private void GameOver()
    {
        Pause();
        UpdateHighScoreUI();
        gameOverScreen.Show(Score, HighScore);
    }
}
