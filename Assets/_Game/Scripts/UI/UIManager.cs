using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private RectTransform healthBarFill;
    [SerializeField] private RectTransform xpBarFill;
    [SerializeField] private TextMeshProUGUI summaryTimeText;
    [SerializeField] private TextMeshProUGUI summaryKillsText;

    private float _healthBarMaxWidth;
    private float _xpBarMaxWidth;
    private SurvivalTimer _timer;

    private void Start()
    {
        GameManager.Instance.OnGameOver.AddListener(ShowGameOver);

        _timer = GameObject.Find("GameManager").GetComponent<SurvivalTimer>();
        _timer.OnTimerUpdated.AddListener(UpdateTimer);

        var restartBtn = gameOverPanel.transform.Find("RestartButton").GetComponent<UnityEngine.UI.Button>();
        restartBtn.onClick.AddListener(() => GameManager.Instance.RestartGame());

        var gameOverMenuBtn = gameOverPanel.transform.Find("MenuButton").GetComponent<UnityEngine.UI.Button>();
        gameOverMenuBtn.onClick.AddListener(() => { Time.timeScale = 1f; SceneManager.LoadScene("MainMenuScene"); });

        var resumeBtn = pausePanel.transform.Find("ResumeButton").GetComponent<UnityEngine.UI.Button>();
        resumeBtn.onClick.AddListener(Resume);

        var menuBtn = pausePanel.transform.Find("MenuButton").GetComponent<UnityEngine.UI.Button>();
        menuBtn.onClick.AddListener(() => { Time.timeScale = 1f; SceneManager.LoadScene("MainMenuScene"); });

        var stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        stats.OnHealthChanged.AddListener(UpdateHealthBar);

        var xp = GameObject.FindWithTag("Player").GetComponent<PlayerXP>();
        xp.OnXPChanged.AddListener(UpdateXPBar);
        xp.OnLevelUp.AddListener(UpdateLevel);

        _healthBarMaxWidth = healthBarFill.rect.width;
        _xpBarMaxWidth = xpBarFill.rect.width;
        xpBarFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        killText.text = $"{GameManager.Instance.KillCount} kills";

        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameManager.Instance.State == GameManager.GameState.Playing)
                Pause();
            else if (GameManager.Instance.State == GameManager.GameState.Paused)
                Resume();
        }
    }

    private void Pause()
    {
        GameManager.Instance.PauseGame();
        pausePanel.SetActive(true);
    }

    private void Resume()
    {
        GameManager.Instance.ResumeGame();
        pausePanel.SetActive(false);
    }

    public void UpdateTimer(float elapsed)
    {
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateHealthBar(float current, float max)
    {
        healthBarFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBarMaxWidth * (current / max));
    }

    private void UpdateXPBar(float current, float required)
    {
        xpBarFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _xpBarMaxWidth * (current / required));
    }

    private void UpdateLevel(int level)
    {
        levelText.text = $"Lv {level}";
    }

    private void ShowGameOver()
    {
        float elapsed = _timer.Elapsed;
        int minutes = Mathf.FloorToInt(elapsed / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);
        summaryTimeText.text = $"Time: {minutes:00}:{seconds:00}";
        summaryKillsText.text = $"Kills: {GameManager.Instance.KillCount}";
        gameOverPanel.SetActive(true);
    }
}
