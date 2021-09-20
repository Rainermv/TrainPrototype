using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SpawnInterval;

    private Transform _enemiesTransform;
    public Entity EntityPrefab;

    public Transform pointA;
    public Transform pointB;

    public float SmoothTime = 0.3f;

    float yVelocity = 0.0f;
    private bool goToPointB = true;
    private Transform target;
    private double maxDistance = 0.1;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnEnemies());
        target = pointA;
    }

    public void Initialize(Transform enemiesTransform)
    {
        _enemiesTransform = enemiesTransform;
    }

    public List<Entity> Spawn(int numberOfUnits, Action<Entity> onEntityDestroyed)
    {
        var entities = new List<Entity>();
        for (var i = 0; i < numberOfUnits; i++)
        {
            var enemy = Instantiate(EntityPrefab, transform.position, Quaternion.identity).GetComponent<Entity>();
            enemy.transform.parent = _enemiesTransform;
            enemy.OnEntityDestroyed += onEntityDestroyed;
            entities.Add(enemy);

        }

        return entities;
    }


    // Update is called once per frame
    void Update()
    {
        float newPosition = Mathf.SmoothDamp(transform.position.y, target.position.y, ref yVelocity, SmoothTime);
        transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
        
        if (PhysicsFunctions.Distance(pointA, transform) > PhysicsFunctions.Distance(pointB, transform) && PhysicsFunctions.Distance(pointB, transform) < maxDistance)
        {
            target = pointA;
            return;
        }

        if (PhysicsFunctions.Distance(pointB, transform) > PhysicsFunctions.Distance(pointA, transform) && PhysicsFunctions.Distance(pointA, transform) < maxDistance)
        {
            target = pointB;
            return;
        }
    }

    
}
