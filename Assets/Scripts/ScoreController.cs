using InfinityEngine.Localization;
using TMPro;
using UnityEngine;
using Zenject;

namespace GravityDJ
{
    public class ScoreController
    {
        [Inject(Id="score")] internal TextMeshProUGUI scoreText;

        [Inject(Id="bestScore")] private TextMeshProUGUI bestScoreText;

        private int? highScore;
        private int score = 0;

        public int HighScore
        {
            get
            {
                ReadHighScoreIfNull();
            
                return highScore.Value;
            }
            private set
            {
                highScore = value;
                SaveHighScore(value);
                UpdateHighScoreUI();
            }
        }
        
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                UpdateHighScore();
                UpdateScoreUI();
            }
        }
        
        private void UpdateHighScore()
        {
            if (HighScore < Score)
            {
                HighScore = Score;
            }
        }

        private void ReadHighScoreIfNull()
        {
            if (!highScore.HasValue)
            {
                highScore = PlayerPrefs.GetInt(nameof(highScore), 0);
            }
        }

        private void SaveHighScore(int value)
        {
            PlayerPrefs.SetInt(nameof(highScore), value);
        }

        private void UpdateHighScoreUI()
        {
            bestScoreText.text = string.Format(R3.strings.BestScoreFormat, HighScore);
        }

        private void UpdateScoreUI()
        {
            scoreText.text = string.Format(R3.strings.ScoreFormat, Score);
        }

        public void Reset()
        {
            Score = 0;
            UpdateHighScoreUI();
        }

        public void UpdateUI()
        {
            UpdateScoreUI();
            UpdateHighScoreUI();
        }
    }
}