using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWaveSystem : MonoBehaviour
{
    [Header("References")] 
    public UnityEngine.UI.Text LevelText;
    public Transform EnemiesTransform;

    [Header("Levels")]
    public float TimeBetweenLevels;
    public int NumberOfWavesInLevel;
    
    [Header("Waves")]
    public float TimeBetweenWaves;
    public float TimeBetweenWavesVariation;

    [Header("Enemies")]
    public int NumberOfEnemiesInWave;
    public int NumberOfEnemiesVariation;

    public Action<int> OnLevelEndAction;

    private List<Spawner> _spawners;

    // Start is called before the first frame update
    void Start()
    {
        _spawners = GetComponentsInChildren<Spawner>().ToList();
        foreach (var spawner in _spawners)
        {
            spawner.Initialize(EnemiesTransform);
        }

        OnLevelEndAction += OnLevelEnd;
        StartCoroutine(StartLevel(0));
    }

    private void OnLevelEnd(int levelCounter)
    {
        StartCoroutine(StartLevelDelayed(++levelCounter));
    }

    private IEnumerator StartLevelDelayed(int levelCounter)
    {
        LevelText.text = $"Loading Level {levelCounter}...";
        yield return new WaitForSeconds(TimeBetweenLevels);
        StartCoroutine(StartLevel(levelCounter));
    }

    private IEnumerator StartLevel(int levelScaling)
    {
        LevelText.text = $"Level {levelScaling}";

        

        //var waveCounter = NumberOfWaves;
        var enemiesMin = NumberOfEnemiesInWave - NumberOfEnemiesVariation + levelScaling;
        var enemiesMax = NumberOfEnemiesInWave + NumberOfEnemiesVariation + levelScaling;

        Debug.Log($"Starting Level {levelScaling} - ({enemiesMin},{enemiesMax})");

        var waveTimeMin = TimeBetweenWaves - TimeBetweenWavesVariation;
        var waveTimeMax = TimeBetweenWaves + TimeBetweenWavesVariation;

        for (var i = 0; i < NumberOfWavesInLevel; i++)
        {
            var numberOfUnits = Random.Range(enemiesMin, enemiesMax);
            RandomSpawner().Spawn(numberOfUnits);

            yield return new WaitForSeconds(Random.Range(waveTimeMin, waveTimeMax));
        }

        OnLevelEndAction(levelScaling);
    }

    private Spawner RandomSpawner()
    {
        return _spawners[Random.Range(0, _spawners.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
