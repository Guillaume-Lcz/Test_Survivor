using UnityEngine;
using UnityEngine.Events;

public class SurvivalTimer : MonoBehaviour
{
    public float Elapsed { get; private set; }

    public UnityEvent<float> OnTimerUpdated; // passes elapsed seconds

private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.State != GameManager.GameState.Playing) return;

        Elapsed += Time.deltaTime;
        OnTimerUpdated?.Invoke(Elapsed);
    }
}
