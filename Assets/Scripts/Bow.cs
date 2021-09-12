﻿using System;
using System.Collections;
using System.Linq;
using System.Numerics;
using Assets.Scripts;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


[RequireComponent(typeof(Entity))]
public class Bow : MonoBehaviour
{
    public float MaxRange;
    public int Damage;
    public float Cooldown;
    public float AimError = 1f;
    public float ArcShotHeightToTarget = 1f;
    public float ArcShotLateralSpeed = 5;
    public double DirectShotDistance = 3f;
    public float DirectShotMaxSpeed = 5f;
    public float DirectShotMinSpeed = 20f;

    public Arrow ArrowPrefab;

    [HideInInspector]
    public GameObject Target;

    private string _opponentTag;


    //private float ShotMinSpeed => SHOT_NORMAL_SPEED * 0.5f;


    public Vector2 DebugDrawTarget { get; set; }

    public void SetHeight(float height)
    {
        ArcShotHeightToTarget = height;
    }
    public void SetSpeed(float speed)
    {
        //SHOT_MIN_SPEED = speed;
    }
    private void Awake()
    {
        _opponentTag = Tags.ENEMY;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScanForTargets());
        StartCoroutine(Attack());
    }

    public bool TargetInRange
    {
        get
        {
            if (Target == null)
            {
                return false;
            }

            //return Fts.ballistic_range(DirectShotMaxSpeed, Physics.gravity.magnitude, transform.position.y) <= MaxRange;

            return Vector2.Distance(Target.transform.position, transform.position) <= MaxRange;
        }
    }

    private IEnumerator ScanForTargets()
    {

        while (true)
        {

            if (Target == null || (Target != null && !TargetInRange))
            {

                var scan = Physics2D.OverlapCircleAll(transform.position, MaxRange);

                if (scan.Length > 0)
                {

                    var closestEnemy = scan.Where(e => e.tag == _opponentTag).OrderBy(e => (e.transform.position - transform.position).sqrMagnitude)
                        .FirstOrDefault();

                    if (closestEnemy != null)
                    {
                        Target = closestEnemy.gameObject;
                    }
                }


            }

            yield return new WaitForSeconds(0.1f);

        }
    }

    

    private IEnumerator Attack()
    {
        while (true)
        {
            if (Target != null)
            {

                var aimPoint = AimToPointWithError(transform.position.x, Target, AimError);
                
                //var lateralSpeed = SHOT_MAX_SPEED;
                if (PhysicsFunctions.LateralDistance(transform.position, aimPoint) < DirectShotDistance)
                {
                    DebugAimPoint(aimPoint, Color.red);
                    // Straight Shot
                    PhysicsFunctions.FindArrowVelocity(this.transform.position, aimPoint, 
                        DirectShotMinSpeed,
                        DirectShotMaxSpeed,
                        Physics.gravity.magnitude,
                        (speed, vector3) =>
                        {
                            Debug.Log($"Found target with speed: [{speed}]");
                            ShootArrow(vector3);
                        }, CannotShootArrow);

                }
                else
                {
                    DebugAimPoint(aimPoint, Color.blue);
                    // Shoot max speed at max distance using lateral velocity
                    PhysicsFunctions.FindArrowVelocityLateral(this.transform.position,
                        aimPoint,
                        ArcShotLateralSpeed,
                        aimPoint.y + ArcShotHeightToTarget,//ArcShotMaxHeight,
                        (speed, vector3, gravity) =>
                        {
                            Debug.Log($"Found target with speed: [{speed}] and gravity [{gravity}]");
                            ShootArrow(vector3, gravity);
                        }, CannotShootArrow);

                }



            }

            yield return new WaitForSeconds(Cooldown);
        }
    }

    private static Vector2 AimToPointWithError(float originX, GameObject target, float aimErrorMagnitude)
    {
        var direction = Math.Sign(target.transform.position.x - originX);
        var randomX = Random.Range(0, aimErrorMagnitude) * direction;
        var randomY = Random.Range(0, aimErrorMagnitude);
        //var circleX = Mathf.Max(0, circle.x);
        return new Vector2(randomX, randomY) + (Vector2)target.transform.position ;

    }


    private void DebugAimPoint(Vector2 aim, Color aimColor)
    {
        Debug.DrawLine(transform.position, aim, aimColor, Cooldown);
    }


    public void CannotShootArrow(float f)
    {
        Debug.Log($"CAN'T HIT TARGET WITH SPEED {f}");
    }


    private void ShootArrow(Vector3 force)
    {
        var arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity).GetComponent<Arrow>();

        arrow.Initialize(force, Physics.gravity.magnitude, Damage, tag, Color.red);
    }

    private void ShootArrow(Vector3 force, float targetGravity)
    {

        var arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity).GetComponent<Arrow>();
        
        arrow.Initialize(force, targetGravity, Damage, tag, Color.white);
    }

    /*
Vector3 calcBallisticVelocityVector(Vector3 source, Vector3 target, float angle) {
    Vector3 direction = target - source;
    float h = direction.y;
    direction.y = 0;
    float distance = direction.magnitude;
    float a = angle * Mathf.Deg2Rad;
    direction.y = distance * Mathf.Tan(a);
    distance += h / Mathf.Tan(a);

    // calculate velocity
    float velocity = Mathf.Sqrt(distance * Physics2D.gravity.magnitude / Mathf.Sin(2 * a));
    return velocity * direction.normalized;
}
*/

    public static Vector2 calcBallisticVelocityVector(Vector3 source, Vector3 target, float angle, float gravity)
    {
        Vector3 direction = target - source;
        var h = direction.y;
        direction.y = 0;
        var distance = direction.magnitude;
        var a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = Mathf.Sqrt(distance * gravity / Mathf.Sin(2 * a));
        return velocity * direction.normalized;
    }

    public static Vector2 calcBallisticVelocityVector1(Vector2 objA, Vector2 objB, float initialAngle,
        float gravity)
    {
        //float gravity = ;
        // Selected angle in radians


        // Planar distance between objects
        Vector2 p1 = new Vector2(objA.x, objA.y);
        Vector2 p2 = new Vector2(objB.x, objA.y);

        // Distance along the x axis between objects
        float distanceX = Vector2.Distance(p1, p2);

        // Distance along the y axis between objects
        float distanceY = objA.y - objB.y;

        float angle = initialAngle * Mathf.Deg2Rad;

        float initialVelocity = (1 / Mathf.Cos(angle)) *
                                Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distanceX, 2)) /
                                           (distanceX * Mathf.Tan(angle) + distanceY));

        Vector2 velocity = new Vector2(initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector2.Angle(Vector2.right, p2 - p1);
        Vector2 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector2.right) * velocity;

        // Fire!
        //rigid.velocity = finalVelocity;
        return finalVelocity;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }
}