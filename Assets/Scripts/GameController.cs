using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int MaxTrainSize;
    public UIController UIController;
    public LevelController LevelController;
    public TextMeshPro GoldPopTextPrefab;
    public TrainController TrainController;

    public WagonComponent[] WagonComponentPrefabs;

    
    private GameData _gameData;
    private List<WagonComponent> _wagonLibrary;
    //private List<WagonComponent> _playerWagons = new List<WagonComponent>();
    
    private const float GridSize = 1.5f;
    
    void Awake()
    {
        _wagonLibrary = WagonComponentPrefabs.ToList();
        _gameData = GameData.InitGameData();
    }
    // Start is called before the first frame update
    void Start()
    {


        // UI CONTROLLER
        UIController.Initialize(_gameData, _wagonLibrary, OnStartLevel);
        UIController.OnBuyWagonButtonClicked = (wagonComponent) =>
        {
            if (TrainController.TrainSize >= MaxTrainSize)
            {
                return;
            }

            if (_gameData.Gold - wagonComponent.WagonPrice > 0)
            {
                AddWagon(wagonComponent);
                _gameData.Gold -= wagonComponent.WagonPrice;
                UIController.UpdateUI(_gameData);
            }
        };
        //UIController.StateStationUI(_gameData);

        // LEVEL CONTROLLER
        LevelController.Initialize(_gameData, OnStartStation);

        InstantiateWagons(_gameData.PlayerWagons);

    }

    void Update()
    {
        switch (_gameData.GameState)
        {
            case GameState.Level:
                LevelController.OnUpdate();
                break;
            case GameState.Station:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    private void InstantiateWagons(List<int> playerWagonsToInstantiate)
    {
        foreach (var playerWagonIndex in playerWagonsToInstantiate)
        {
            var wagonPrefab = _wagonLibrary[playerWagonIndex];
            AddWagon(wagonPrefab);
        }
    }

    private void AddWagon(WagonComponent wagonPrefab)
    {
        var wagon = TrainController.AddWagonOnRear(wagonPrefab);
        
        // DragSlot
        UIController.AddDraggableSlot(wagon.transform.position, wagon, wagon.WagonName);
    }

    private void OnStartLevel()
    {
        _gameData.GameState = GameState.Level;
        _gameData.Level++; //Proceed to next level 

        UIController.UpdateUI(_gameData);
        LevelController.StartAcceleratingToBeginLevel(_gameData.Level);
    }

    private void OnStartStation()
    {
        var cargoWagons = TrainController.GetComponentsInChildren<Cargo>().ToList();

        _gameData.GameState = GameState.Station;
        _gameData.Gold += cargoWagons.Sum(cargo => cargo.GoldValue);


        UIController.UpdateUI(_gameData);

        foreach (var cargoWagon in cargoWagons)
        {
            StartCoroutine(DisplayGoldValueRoutine(cargoWagon));
        }
        // wait for player to click the start button
    }

    private IEnumerator DisplayGoldValueRoutine(Cargo cargoWagon)
    {
        var goldPop = Instantiate(GoldPopTextPrefab, cargoWagon.transform.position, cargoWagon.transform.rotation);
        goldPop.text = $"{cargoWagon.GoldValue}";

        var t = 0f;
        var yStart = goldPop.transform.position.y;
        var yTarget = yStart + 0.7f;
        var time = 0f;

        float alphaStart = 1.5f;
        float alphaTarget = 0f;

        while (goldPop.transform.position.y < yTarget)
        {
            var position = goldPop.transform.position;

            position.y = Mathf.Lerp(yStart, yTarget, time * 0.3f);
            goldPop.transform.position = position;

            goldPop.alpha = Mathf.Lerp(alphaStart, alphaTarget, time * 0.5f );

            time += Time.deltaTime;

            yield return null;
        }

        Destroy(goldPop.gameObject);

    }

}
