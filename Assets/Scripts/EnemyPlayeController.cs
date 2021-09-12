using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPlayeController : MonoBehaviour
{
    private Castle _enemyCastle;
    private float _actionMinTime;
    private float _actionMaxTime;

    InterfaceController _interfaceController;



    // Start is called before the first frame update
    public void Initialize(InterfaceController interfaceController, Castle enemyCastle, float actionMinTime, float actionMaxTime) {

        _enemyCastle = enemyCastle;

        _actionMinTime = actionMinTime;
        _actionMaxTime = actionMaxTime;

        _interfaceController = interfaceController;
        StartCoroutine(EnemyBehaviour());
    }

    private IEnumerator EnemyBehaviour() {

        var laneCount = _interfaceController.Lanes.Length;
        var emojiCount = _interfaceController.emojiPrefabs.Length;

        while (_enemyCastle != null && _enemyCastle.isActiveAndEnabled) {

            yield return new WaitForSeconds(Random.Range(_actionMinTime, _actionMaxTime));

            var lane = _interfaceController.Lanes[Random.Range(0, laneCount)];
            var emoji = _interfaceController.emojiPrefabs[Random.Range(0, emojiCount)];

            _interfaceController.InstantiateOnLane(emoji, _enemyCastle, lane, Tags.ENEMY);
        }


        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
