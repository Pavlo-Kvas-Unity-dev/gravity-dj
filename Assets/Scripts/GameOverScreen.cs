using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;

    public void Show(int score)
    {
        gameObject.SetActive(true);
        gameOverText.text = $"Game over. \nYou scored {score} points";
    }
}
