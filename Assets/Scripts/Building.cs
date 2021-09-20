using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float MoveTimeScale = 1;
    private float _speed;
    private float _acceleration;

    [HideInInspector]
    //public float BaseSpeed { get; set; }
    public Action OnReachedLimit { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

   
    public void Initialize(Vector2 screenPosition)
    {
        transform.position = screenPosition;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }


    public void StartMovingTo(Vector2 position, Action onReachedTargetPosition)
    {
        StartCoroutine(StartMovingToRoutine(position, onReachedTargetPosition));
    }

    public void UpdateValues(float playerSpeed, float playerAcceleration)
    {
        _speed = playerSpeed;
        _acceleration = playerAcceleration;
    }

    private IEnumerator StartMovingToRoutine(Vector2 targetPosition, Action onReachedTargetPosition)
    {
        var initialPosition = transform.position.x;
        var time = 0f;
        //var initialDistance = Vector2.Distance(initialPosition, targetPosition);

        var accelerating = _speed <= 0;

        var velocity = 0f;

        while (Vector2.Distance(targetPosition, transform.position) >= 0.1f) 
        {
            time += Time.deltaTime;

            // this is very stupid
            if (accelerating)
            {
                transform.position = new Vector3(
                    Mathf.Lerp(initialPosition, targetPosition.x, time * _acceleration * (1 / MoveTimeScale)), 
                    transform.position.y, 1);
            }
            else
            {
                transform.position = new Vector3(
                    Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity, 0.9f),
                    transform.position.y, 1);
            }

            yield return null;

        }

        onReachedTargetPosition();
    }

    
}
