using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public UIController UIController;
    public LevelController LevelController;
    public GameObject PlayerGameObject;
    public TextMeshPro GoldPopTextPrefab;

    private GameData _gameData;

    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameData = GameData.InitGameData();

        // UI CONTROLLER
        UIController.Initialize(_gameData, OnStartLevel);
        //UIController.StateStationUI(_gameData);

        // LEVEL CONTROLLER

        LevelController.Initialize(_gameData, OnStartStation);


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
        var cargoWagons = PlayerGameObject.GetComponentsInChildren<Cargo>();

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