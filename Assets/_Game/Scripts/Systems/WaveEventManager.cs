using System.Collections.Generic;
using UnityEngine;

public class WaveEventManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;

    // Timeline entry: trigger time in seconds + event to fire
    private readonly List<(float time, IWaveEvent waveEvent)> _timeline = new();
    private float _elapsed;
    private int _nextIndex;

    private void Start()
    {
        BuildTimeline();
        _timeline.Sort((a, b) => a.time.CompareTo(b.time));
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;

        while (_nextIndex < _timeline.Count && _elapsed >= _timeline[_nextIndex].time)
        {
            _timeline[_nextIndex].waveEvent.Execute(spawner);
            _nextIndex++;
        }
    }

    private void BuildTimeline()
    {
        // Events disabled for now — add entries here when HordeEvent / EliteEvent / BossEvent are ready
        // Example:
        // _timeline.Add((60f, new HordeEvent(...)));
        // _timeline.Add((120f, new EliteEvent(...)));
        // _timeline.Add((300f, new BossEvent(...)));
    }
}
