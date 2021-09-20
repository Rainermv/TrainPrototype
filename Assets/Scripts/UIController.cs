using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [Header("Station UI")]
    public GameObject StationUI;
    public UnityEngine.UI.Text TitleText;
    public UnityEngine.UI.Text NextLevelButtonText;
    public UnityEngine.UI.Button NextLevelButton;

    [Header("Level UI")]
    public GameObject LevelUI;

    private int _tier;
    private UIState _state;

    private Action<int> _onNextLevelButtonClicked;


    public void Initialize(UIState uiState, Action<int> onNextLevelButtonClicked)
    {
        SetState(uiState);
        _onNextLevelButtonClicked = onNextLevelButtonClicked;
    }

    public void SetState(UIState state)
    {
        SetUiStatus(false);
        _state = state;
        switch (_state)
        {
            case UIState.Level:
                LevelUI.SetActive(true);
                break;
            case UIState.Station:
                StationUI.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetUiStatus(bool isActive)
    {
        StationUI.gameObject.SetActive(isActive);
        LevelUI.gameObject.SetActive(isActive);
    }

    public void InitializeStationUI(int level, int nextLevel)
    {
        TitleText.text = $"Welcome to Station {level}";
        _tier = level / 3;
        NextLevelButtonText.text = $"Press to start level {nextLevel}";
        
        NextLevelButton.onClick.RemoveAllListeners(); // HAAAACK!!
        NextLevelButton.onClick.AddListener(() =>
        {
            _onNextLevelButtonClicked(nextLevel);
        });

        SetState(UIState.Station);

    }

    
}
