using System;
using InfinityEngine.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GravityDJ
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Button restartButton;
        
        public event Action RestartGame;
        
        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            Hide();
            RestartGame.Invoke();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(ScoreController scoreController)
        {
            gameObject.SetActive(true);
            gameOverText.text = string.Format(R3.strings.GameOver, scoreController.Score);
        }
    }
}
