using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text winnerText;  
    
    void CanvasSetting()
    {
        scoreText.text = $"Blue {GameScoreManager.Instance.bluePlayerScore} vs Pink {GameScoreManager.Instance.pinkPlayerScore}";
        // Blue Player Win !!!
        winnerText.text = (GameScoreManager.Instance.bluePlayerScore > GameScoreManager.Instance.pinkPlayerScore)
            ? "Blue Player Win !!!" : "Pink Player Win !!!";
    }
}
