using UnityEngine;
using TMPro;

    /// <summary>
    /// Updates the moves and score texts
    /// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void ResetUI(int currentMoves, int currentScore)
    {
        UpdateMovesText(currentMoves);
        UpdateScoreText(currentScore);
    }

    public void UpdateMovesText(int currentMoves)
    {
        if (movesText != null)
        {
            movesText.text = $"{currentMoves}";
        }
    }

    public void UpdateScoreText(int currentScore){
        if (scoreText != null) {
            scoreText.text = $"{currentScore}";
        }
    }
}
