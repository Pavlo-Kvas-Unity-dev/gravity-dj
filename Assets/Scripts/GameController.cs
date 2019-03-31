using System;
using GravityDJ.UI;
using InfinityEngine.Localization;
using TMPro;
using UnityEngine;
using Zenject;
using Button = UnityEngine.UI.Button;


namespace GravityDJ
{
    public class GameController : MonoBehaviour
    {
        [Inject(Id="gameCountdown")] private TextMeshProUGUI countdownText;

        [Inject(Id="mainMenuButton")] private Button mainMenuButton;

        [Inject] private GameOverScreen gameOverScreen;

        [Inject] private HelpWindow helpWindow;

        [Inject] private MainMenuWindow mainMenuWindow;

        Settings settings;

        private Spawner spawner;

        private GravityController gravityController;
        
        private ScoreController scoreController;

        private bool isGamePlaing = false;

        private float countdown;

        [Inject]
        void Init(Settings settings, Spawner spawner, GravityController gravityController, ScoreController scoreController)
        {
            this.settings = settings;
            this.spawner = spawner;
            this.gravityController = gravityController;
            this.scoreController = scoreController;
        }
    
        void Awake()
        {
            ISILocalization.onLanguageChanged += OnLanguageChanged;
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            gameOverScreen.RestartGame += StartGame;
            spawner.ballSpawned += OnBallSpawned;
        }

        private void OnBallSpawned(Ball ball)
        {
            ball.targetHit += OnTargetHit;
        }

        void Start()
        {
            ShowMainMenu();
        }

        private void FixedUpdate()
        {
            if (isGamePlaing)
            {
                UpdateCountdown();

                if (countdown < 0)
                {
                    GameOver();
                }
            }
        }

        private void UpdateCountdown()
        {
            UpdateCountdownUI();
            countdown -= Time.deltaTime;
        }

        private void OnLanguageChanged()
        {
            scoreController.UpdateUI();
            UpdateCountdownUI();
        }

        private void UpdateCountdownUI()
        {
            countdownText.text = string.Format(R3.strings.TimeLeftFormat, (int) countdown);
        }

        private void StartGame()
        {
            gravityController.Reset();
            ResetCountdown();
            Resume();
            scoreController.Reset();
            spawner.Spawn();
        }

        private void ResetCountdown()
        {
            countdown = settings.gameDuration;
        }

        private void OnMainMenuButtonClicked()
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

        private void Resume()
        {
            Time.timeScale = 1;
            isGamePlaing = true;
        }

        public void OnTargetHit()
        {
            scoreController.Score++;
        }

        private void GameOver()
        {
            Pause();
            gameOverScreen.Show(scoreController);
        }

        [Serializable]
        public class Settings
        {
            public float gameDuration = 60f;
        }
    }
}
