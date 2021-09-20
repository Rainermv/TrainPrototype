using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [Header("Station UI")]
    public GameObject StationUI;
    public UnityEngine.UI.Text TitleText;
    public UnityEngine.UI.Text GoldText;
    public UnityEngine.UI.Text NextLevelButtonText;
    public UnityEngine.UI.Button NextLevelButton;

    [Header("Level UI")]
    public GameObject LevelUI;

    private int _tier;

    private Action _onNextLevelButtonClicked;


    public void Initialize(GameData gameData, Action onNextLevelButtonClicked)
    {
        UpdateUI(gameData);
        _onNextLevelButtonClicked = onNextLevelButtonClicked;
    }

    public void UpdateUI(GameData gameData)
    {
        SetUiChildrenStatus(false);
        switch (gameData.GameState)
        {
            case GameState.Level:
                UpdateLevelUI(gameData);
                LevelUI.SetActive(true);
                break;
            case GameState.Station:
                UpdateStationUI(gameData);
                StationUI.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    

    private void SetUiChildrenStatus(bool isActive)
    {
        StationUI.gameObject.SetActive(isActive);
        LevelUI.gameObject.SetActive(isActive);
    }

    private void UpdateLevelUI(GameData gameData)
    {
        //nothing for now
    }

    private void UpdateStationUI(GameData gameData)
    {
        TitleText.text = $"Welcome to Station {gameData.Level}";
        //_tier = gameData.Tier;
        NextLevelButtonText.text = $"Press to start level {gameData.Level+1}";
        GoldText.text = $"Gold: {gameData.Gold}";

        NextLevelButton.onClick.RemoveAllListeners();
        NextLevelButton.onClick.AddListener(() =>
        {
            _onNextLevelButtonClicked();
        });
    }

}
