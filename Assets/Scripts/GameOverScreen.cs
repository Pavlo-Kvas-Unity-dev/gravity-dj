using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverScreen : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button restartButton;

    [Inject]
    public void Init(GameController gameController)
    {
        this.gameController = gameController;
    }
    
    private void Awake()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        Hide();
        gameController.OnRestart();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(int score, int bestScore)
    {
        gameObject.SetActive(true);
        gameOverText.text = $"Game over. \nYou scored {score} points";
    }
}
