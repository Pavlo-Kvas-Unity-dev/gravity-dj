using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private int score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreUI();
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
}
