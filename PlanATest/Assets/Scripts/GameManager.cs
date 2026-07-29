using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager UIObject;
    [SerializeField] private GameOverManager gameOverObject;

    [Header("Start Game Values")]
    [SerializeField] private int initialMoves = 5;
    [SerializeField] private int initialScore = 0;

    private int currentMoves;
    private int currentScore;

    public int GetMoves() => currentMoves;
    public int GetScore() => currentScore;

    private void Start()
    {
        Reset();
    }


    public void Reset()
    {
        currentMoves = initialMoves;
        currentScore = initialScore;

        UIObject.ResetUI(currentMoves, currentScore);
        gameOverObject.HideGameOver();
    }

    public void SetMoves(int value)
    {
        currentMoves = Mathf.Max(0, value);
        UIObject.UpdateMovesText(currentMoves);
    }

    public void DecreaseMoves(int amount = 1)
    {
        currentMoves = Mathf.Max(0, currentMoves - amount);
        UIObject.UpdateMovesText(currentMoves);
        gameOverObject.CheckGameOver(currentMoves);
    }

    public void SetScore(int value)
    {
        currentScore = value;
        UIObject.UpdateScoreText(currentScore);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UIObject.UpdateScoreText(currentScore);
    }
}