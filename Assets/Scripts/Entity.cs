using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Entity : MonoBehaviour
{
    public EntityMovementType EntityMovementType;
    
    public EntityTag[] EntityTags;

    public float Speed;
    public int Health;

    public float FlyByMoveAdjacentMagnitude = 0;
    public float FlyByWaitingTime = 1;
    public float FlyByMaxHeightFromPoint = 1;


    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer[] _spriteRenderers;
    private Color[] _originalColors;
    
    private int maxHealth;
    private float directionMultiplier = 1;
    private List<EntityTag> _entityTags;


    private Vector2 V2Position => (Vector2)transform.position;

    private void Awake() {

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _originalColors = _spriteRenderers.Select(rend => rend.color).ToArray();

        _entityTags = EntityTags.ToList();

        maxHealth = Health;

    }

    void Start()
    {

        if (EntityMovementType == EntityMovementType.FlyBy)
        {
            StartCoroutine(FlyByMovement());
        }
        
    }

    private IEnumerator FlyByMovement()
    {
        var flyByState = 0;
        var movePointsContainer = FindObjectOfType<MovePointsContainer>();

        //1. Find a point inside screenspace
        var movePoints = movePointsContainer.GetRandomPointSequenceWithExit(3);
        Debug.Log($"Move points: {string.Join(", ", movePoints.Select(movePoint => movePoint.name))}");
        var nextMoveTransform = GetFirstMoveTransform(movePoints);
        var adjustedMovePoint = GetRandomMovePointAroundTransform(nextMoveTransform);
        var velocity = Vector2.zero;

        while (true)
        {
            if (V2Position == (Vector2)adjustedMovePoint)
            {
                //3. Stays still for some time
                Debug.Log($"Waiting for {FlyByWaitingTime}");
                yield return new WaitForSeconds(FlyByWaitingTime);

                movePoints.RemoveAt(0);

                //4. Go to next point
                nextMoveTransform = GetFirstMoveTransform(movePoints);
                if (nextMoveTransform == null)
                {
                    break;
                }
                adjustedMovePoint = GetRandomMovePointAroundTransform(nextMoveTransform);
            }

            transform.position = Vector2.SmoothDamp(V2Position, adjustedMovePoint, ref velocity, 0.5f);
            yield return null;

        }


    }

    private Vector2 GetRandomMovePointAroundTransform(Transform nextMoveTransform)
    {
        return Random.insideUnitCircle * Random.Range(1, FlyByMoveAdjacentMagnitude) + (Vector2)nextMoveTransform.position;
    }

    private IEnumerator FlyByMovement1()
    {

        var flyByState = 0;

        var movePointsContainer = FindObjectOfType<MovePointsContainer>();

        //1. Find a point inside screenspace
        var movePoints = movePointsContainer.GetRandomPointSequenceWithExit(3);
        Debug.Log($"Move points: {string.Join(", ", movePoints.Select(movePoint => movePoint.name))}");
        var nextMovePoint = GetFirstMovePoint(movePoints);
        while (true)
        {
            switch (flyByState)
            {
                case 0: // Find a a point and start moving

                    if (nextMovePoint == V2Position)
                    {
                        flyByState = 1;
                        movePoints.RemoveAt(0);
                        break;
                    }

                    //1.1 find a randomized point adjacent to that
                    //nextMovePoint = Random.insideUnitCircle * Random.Range(1, FlyByMoveAdjacentMagnitude) + nextMovePoint;

                    //2. Fly towards it in a smoothed arc 
                    Fts.solve_ballistic_arc_lateral(V2Position, Speed, nextMovePoint,
                        V2Position.y + FlyByMaxHeightFromPoint, out Vector3 force, out var gravity);

                    _rigidbody2D.gravityScale = PhysicsFunctions.GravityScale(gravity, Physics.gravity.magnitude);
                    _rigidbody2D.AddForce(force * _rigidbody2D.mass, ForceMode2D.Impulse);
                    flyByState = 1;
                    movePoints.RemoveAt(0);
                    break;

                case 1: // Moving towards point
                    if (PhysicsFunctions.LateralDistance(V2Position, nextMovePoint) < 1)
                    {
                        //3.1 Stop
                        _rigidbody2D.velocity = Vector2.zero;
                        _rigidbody2D.gravityScale = 0;
                        //3. Stays still for some time
                        yield return new WaitForSeconds(FlyByWaitingTime);

                        //4. Go to next point
                        nextMovePoint = GetFirstMovePoint(movePoints);
                        if (nextMovePoint == Vector2.zero)
                        {
                            Destroy(gameObject);
                        }
                        flyByState = 0;
                    }
                    break;
            }

            yield return new WaitForSeconds(0.5f);
            
        }



    }

    private Vector2 GetFirstMovePoint(List<Transform> movePoints)
    {
        var movePointTransform = movePoints.FirstOrDefault();
        Debug.Log($"Move points: {string.Join(", ", movePoints.Select(movePoint => movePoint.name))}");
        Debug.Log($"Moving to {movePointTransform.name}");

        return movePointTransform.position;
    }

    private Transform GetFirstMoveTransform(List<Transform> movePoints)
    {
        var movePointTransform = movePoints.FirstOrDefault();
        Debug.Log($"Move points: {string.Join(", ", movePoints.Select(movePoint => movePoint.name))}");
        Debug.Log($"Moving to {movePointTransform.name}");

        return movePointTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health < 0) {
            Die();
            return;
        }

        if (_rigidbody2D == null)
        {
            return;
        }


        switch (EntityMovementType)
        {
            case EntityMovementType.Horizontal:
                _rigidbody2D.velocity = new Vector2(Speed, 0);
                break;
            case EntityMovementType.FlyBy:
                // Handled by Coroutine
                break;
            case EntityMovementType.Stationary:
                //Does not move
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        


    }

    private void Die() {

        gameObject.SetActive(false);
        Destroy(gameObject);

    }



    public void DoDamage(int damage) {
        Health -= damage;

        for (var i = 0; i < _spriteRenderers.Length; i++)
        {
            var spriteRenderer = _spriteRenderers[i];
            var originalColor = _originalColors[i];
            spriteRenderer.color = Color.Lerp(Color.black, originalColor, (float)Health / (float)maxHealth);
        }

    }


    public bool CanBeTargetedBy(List<EntityTag> targetEntityTags)
    {
        return targetEntityTags.Any(tag => EntityTags.Contains(tag));
    }
}