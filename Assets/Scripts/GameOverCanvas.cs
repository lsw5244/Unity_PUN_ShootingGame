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

    public void CanvasSetting(int bluePlayerScore, int pinkPlayerScore)
    {
        scoreText.text = $"Blue {bluePlayerScore} vs Pink {pinkPlayerScore}";
        winnerText.text = (bluePlayerScore > pinkPlayerScore)
            ? "Blue Player Win !!!" : "Pink Player Win !!!";
    }
}