using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private float gameOverDelay = 1.0f;
    public bool isGameOver { get; private set; } 
    

    private void Start(){
        isGameOver = false;
        HideGameOver();
    }

    public void CheckGameOver(int currentMoves, int score)
    {
        if (currentMoves <= 0 && !isGameOver)
        {
            TriggerGameOver(score);
        }
    }

    public async Task TriggerGameOver(int score)
    {
        isGameOver = true;

        int millisecondsDelay = Mathf.RoundToInt(gameOverDelay * 1000f);
        await Task.Delay(millisecondsDelay);
        if (panel != null)
        {
            panel.SetActive(true);
        }
        if (finalScoreText != null) {
            finalScoreText.text = $"{score}";
        }
    }

    public void HideGameOver()
    {
        isGameOver = false;

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
