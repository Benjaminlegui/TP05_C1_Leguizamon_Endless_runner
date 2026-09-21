using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text speedText;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private string scoreLabel = "Obstacles";
    [SerializeField] private string speedLabel = "Speed";
    
    private void OnEnable()
    {
        gameManager.Changed += Refresh;
        playButton.onClick.AddListener(gameManager.StartGame);
        retryButton.onClick.AddListener(gameManager.StartGame);
        Refresh();
    }

    private void OnDisable()
    {
        gameManager.Changed -= Refresh;
        playButton.onClick.RemoveListener(gameManager.StartGame);
        retryButton.onClick.RemoveListener(gameManager.StartGame);
    }

    private void Refresh()
    {
        scoreText.text = $"{scoreLabel} {gameManager.Score}";
        speedText.text = $"{speedLabel} {gameManager.CurrentSpeed:0.0}";
        startPanel.SetActive(gameManager.State == GameManager.GameState.Ready);
        gameOverPanel.SetActive(gameManager.State == GameManager.GameState.GameOver);
    }
}
