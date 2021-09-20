using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UIController UIController;
    public LevelController LevelController;


    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {

        // UI CONTROLLER
        UIController.Initialize(UIState.Station, (nextLevel =>
        {
            UIController.SetState(UIState.Level);
            LevelController.StartAcceleratingToBeginLevel(nextLevel);
        }));

        UIController.InitializeStationUI(0, 1);

        // LEVEL CONTROLLER

        LevelController.Initialize(0, (level, nextLevel) =>
        {
            UIController.InitializeStationUI(level, nextLevel);
        });

        //LevelController.StartAcceleratingToBeginLevel(initialLevel);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum UIState
{
    Level,
    Station
}
