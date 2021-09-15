using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovePointsContainer : MonoBehaviour
{

    public Transform MovePointExit;
    public List<Transform> MovePoints;

    void Awake()
    {
        MovePoints = transform.GetComponentsInChildren<Transform>().ToList();

        // remove own transform
        MovePoints.Remove(transform);
    }

    public List<Transform> GetRandomPointSequenceWithExit(int number)
    {
        var random = new System.Random();
        var randomPoints = MovePoints.ToList().OrderBy(a => random.Next()).Take(number).ToList();
        randomPoints.Add(MovePointExit);
        return randomPoints;

    }
}
