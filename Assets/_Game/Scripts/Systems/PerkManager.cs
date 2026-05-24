using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerkManager : MonoBehaviour
{
    [SerializeField] private PerkSO[] perkPool;
    [SerializeField] private int choiceCount = 3;

    public List<PerkSO> CurrentChoices { get; private set; } = new();
    public UnityEvent OnPerkChoicesReady;

    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _player.GetComponent<PlayerXP>().OnLevelUp.AddListener(OnLevelUp);
    }

    private void OnLevelUp(int level)
    {
        CurrentChoices = PickRandom(choiceCount);
        GameManager.Instance.PauseGame();
        OnPerkChoicesReady.Invoke();
    }

    public void SelectPerk(PerkSO perk)
    {
        perk.Apply(_player);
        GameManager.Instance.ResumeGame();
    }

    private List<PerkSO> PickRandom(int count)
    {
        var pool = new List<PerkSO>(perkPool);
        var choices = new List<PerkSO>();
        count = Mathf.Min(count, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            choices.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return choices;
    }
}
