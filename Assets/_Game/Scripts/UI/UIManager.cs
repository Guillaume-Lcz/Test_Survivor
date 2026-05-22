using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        GameManager.Instance.OnGameOver.AddListener(ShowGameOver);

        var timer = GameObject.Find("GameManager").GetComponent<SurvivalTimer>();
        timer.OnTimerUpdated.AddListener(UpdateTimer);

        var restartBtn = gameOverPanel.transform.Find("RestartButton").GetComponent<UnityEngine.UI.Button>();
        restartBtn.onClick.AddListener(() => GameManager.Instance.RestartGame());
    }

    public void UpdateTimer(float elapsed)
    {
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
