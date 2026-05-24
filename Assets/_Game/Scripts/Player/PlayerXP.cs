using UnityEngine;
using UnityEngine.Events;

public class PlayerXP : MonoBehaviour
{
    [SerializeField] private float xpPerLevel = 100f;
    [SerializeField] private float xpScalingFactor = 1.2f;

    public int Level { get; private set; } = 1;
    public float CurrentXP { get; private set; }
    public float XPToNextLevel { get; private set; }

    public UnityEvent<float, float> OnXPChanged; // current, required
    public UnityEvent<int> OnLevelUp;            // new level

    private void Awake()
    {
        XPToNextLevel = xpPerLevel;
    }

    public void AddXP(float amount)
    {
        CurrentXP += amount;
        while (CurrentXP >= XPToNextLevel)
        {
            CurrentXP -= XPToNextLevel;
            Level++;
            XPToNextLevel *= xpScalingFactor;
            OnLevelUp?.Invoke(Level);
        }
        OnXPChanged?.Invoke(CurrentXP, XPToNextLevel);
    }
}
