using UnityEngine;
/// <summary>
/// Controls the game loop and input functions
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager UIObject;
    [SerializeField] private GameOverManager gameOverObject;
    [SerializeField] private GridManager gridObject;


    [Header("Start Game Values")]
    [SerializeField] private int initialMoves = 5;
    [SerializeField] private int initialScore = 0;
    
    public int currentMoves { get; private set; } 
    public int currentScore { get; private set; } 


    private void Start()
    {
        gridObject.GenerateGrid();
        gridObject.OnComboCollected += HandleComboCollected;
        Reset();
    }


    public void Reset()
    {
        currentMoves = initialMoves;
        currentScore = initialScore;

        UIObject.ResetUI(currentMoves, currentScore);
        gridObject.ResetGrid();
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
        gameOverObject.CheckGameOver(currentMoves, currentScore);
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

    private bool HandleComboCollected(int blocksCollected)
    {
        AddScore(blocksCollected);
        DecreaseMoves();
        return gameOverObject.isGameOver;
    }
}