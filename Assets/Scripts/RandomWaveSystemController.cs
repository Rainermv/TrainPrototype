using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWaveSystemController : MonoBehaviour
{
    [Header("References")] 
    public Transform EnemiesTransform;
    public Transform Spawners;

    [Header("Waves")]
    public float TimeBetweenWaves;
    public float TimeBetweenWavesVariation;

    [Header("Enemies")]
    public int NumberOfEnemiesInWave;
    public int GlobalScaling;
    
    private List<Spawner> _spawners;
    private Action<int, int> _onWaveEnds;
    private Action<int> _onUpdateUnitCount;
    private List<Entity> _enemiesInCurrentWave;

    // Start is called before the first frame update
    void Awake()
    {
        _spawners = Spawners.GetComponentsInChildren<Spawner>().ToList();
        foreach (var spawner in _spawners)
        {
            spawner.Initialize(EnemiesTransform);
        }
    }

    public void Initialize(Action<int> onUpdateUnitCount)
    {
        _onUpdateUnitCount = onUpdateUnitCount;
    }


    private Spawner RandomSpawner()
    {
        return _spawners[Random.Range(0, _spawners.Count)];
    }

    public void GenerateWave(int level, int wave, Action onWaveEnds)
    {
        var enemiesMin = Mathf.Max(1, NumberOfEnemiesInWave - GlobalScaling + level);
        var enemiesMax = Mathf.Min(2, NumberOfEnemiesInWave + GlobalScaling + level + Random.Range(0, wave));

        var numberOfUnits = Random.Range(enemiesMin, enemiesMax);

        var enemiesInWave = new List<Entity>();
        enemiesInWave.AddRange(RandomSpawner().Spawn(numberOfUnits, entity =>
        {
            enemiesInWave.Remove(entity);
            _onUpdateUnitCount(enemiesInWave.Count);

            if (enemiesInWave.Count <= 0)
            {
                onWaveEnds();
            }
        }));

        _onUpdateUnitCount(enemiesInWave.Count);
    }


}
