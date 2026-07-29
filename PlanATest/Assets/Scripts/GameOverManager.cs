using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject panel;

    private bool isGameOver = false;

    private void Start(){
        HideGameOver();
    }

    public void CheckGameOver(int currentMoves)
    {
        if (currentMoves <= 0 && !isGameOver)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        if (panel != null)
        {
            panel.SetActive(true);
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
