using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("UI References")]
    public UnityEngine.UI.Text LevelText;
    public UnityEngine.UI.Text SpeedText;
    public UnityEngine.UI.Text WaveText;
    public UnityEngine.UI.Text TravelText;
    public UnityEngine.UI.Text EnemiesText;

    [Header("Other References")]
    public RandomWaveSystemController WaveSystemController;
    public Transform Backgrounds;
    public Transform Buildings;
    public Transform StationStart;
    public Transform StationStop;
    public Transform StationEnd;
    public Building Station;


    //[Header("Prefabs")]

    [Header("Main Parameters")] 
    public float FullSpeed;
    public int NumberOfWavesInLevel;
    public float TimeBetweenWaves;
    public float BackgroundScale = 10;
    
    private GameObject _station;
    private List<ScrollingBackgroundController> _backgrounds;

    public Action<int, int> OnStationStartAction { get; set; }


    private float _playerSpeed;
    private float _playerAcceleration;
    private float _playerWorldPosition = 0.5f;

    private const float StoppedSpeed = 0f; //duh

    private int _level = 0;
    
    public void Initialize(int initialLevel, Action<int, int> onStationStartAction)
    {
        _level = initialLevel;
        _backgrounds = Backgrounds.GetComponentsInChildren<ScrollingBackgroundController>().ToList();
        _backgrounds.ForEach(background => background.Scale = BackgroundScale);
        OnStationStartAction = onStationStartAction;

        WaveSystemController.Initialize((i => { EnemiesText.text = $"Enemies: {i}"; }));

        SetSpeed(StoppedSpeed); // Initial speed is zero (stopped start)
    }

    void Update()
    {
        _playerWorldPosition += _playerSpeed * Time.deltaTime;
        
        _backgrounds.ForEach(background => background.UpdateScreenPosition(_playerWorldPosition));
        Station.UpdateValues(_playerSpeed, _playerAcceleration);

        if (_playerWorldPosition < 1000)
        {
            TravelText.text = $"Travelled: {_playerWorldPosition:0}m";
            return;
        }

        TravelText.text = $"Travelled: {UnitConvert.UnitsToKm(_playerWorldPosition):0.00}km";
        
    }

    private IEnumerator WaitForSeconds(float seconds, Action OnWaitEndsAction)
    {
        yield return new WaitForSeconds(seconds);
        OnWaitEndsAction();
    }


    private void SetSpeed(float levelSpeed)
    {
        _playerSpeed = levelSpeed;
       
        SpeedText.text = $"Speed: " +
                         $"{UnitConvert.UnitSpeedToKmHour(_playerSpeed):0}km/h";

    }

    public void StartAcceleratingToBeginLevel(int level)
    {
        _level = level;
        LevelText.text = $"Level: {level}";
        Station.StartMovingTo(StationEnd.position,
            () => Station.Disable());
        StartCoroutine(ChangeAccelerationToTargetSpeedRoutine(FullSpeed, () =>
        {
            BeginNextWave(0);
        }));
    }

    private void BeginNextWave(int wave)
    {
        // If we do all the waves, end level
        if (wave >= NumberOfWavesInLevel)
        {
            WaveText.text = $"No more waves";
            EndLevel();
            return;
        }

        WaveText.text = $"Wave: {wave + 1}/{NumberOfWavesInLevel}";

        // if we have not done all waves, generate the next one (after some time)
        WaveSystemController.GenerateWave(_level, wave, () =>
        {
            // increase wave
            wave += 1;
            WaveText.text = $"Waiting for wave: {wave}";
            StartCoroutine(WaitForSeconds(TimeBetweenWaves, () => BeginNextWave(wave)));
        });
    }

    


    private void EndLevel()
    {
        //Station = Instantiate(Station);
        Station.Initialize(StationStart.position);
        Station.StartMovingTo(StationStop.position, () =>
        {
            // do nothing for now
        });
        StartCoroutine(ChangeAccelerationToTargetSpeedRoutine(StoppedSpeed, () => BeginStation()));

    }

    

    private void BeginStation()
    {
       OnStationStartAction(_level, _level +1);
    }


    private IEnumerator ChangeAccelerationToTargetSpeedRoutine(float targetSpeed, Action onTargetSpeedReached)
    {
        var initialSpeed = _playerSpeed;

        var initialAcceleration = 0f;
        var endAcceleration = 1f;
        //var endAcceleration = Mathf.Sign(targetSpeed - _playerSpeed)* 1f;

        var timeAdjustment = 0.2f;
        float accelerationSmoothTime = 1.5f;
        var time = 0f;

        while (true)
        {
            // Lerp
            _playerAcceleration = Mathf.LerpUnclamped(initialAcceleration, endAcceleration, time * accelerationSmoothTime);
            var speed = Mathf.Lerp(initialSpeed, targetSpeed, time * _playerAcceleration);
            time += Time.deltaTime * timeAdjustment;

            SetSpeed(speed);

            var roundedSpeed = (float)System.Math.Round(_playerSpeed, 2);

            if (Mathf.Abs(targetSpeed - roundedSpeed) <= 0.1 )
            {
                SetSpeed(targetSpeed);
                onTargetSpeedReached();
                yield break;
            }

            yield return null;
        }
    }

    
}