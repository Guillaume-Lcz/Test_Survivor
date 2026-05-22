using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        var stats = player.GetComponent<PlayerStats>();
        if (stats == null) return;

        stats.OnDeath.AddListener(OnPlayerDeath);
    }

    private void OnPlayerDeath()
    {
        GameManager.Instance.TriggerGameOver();
    }
}
