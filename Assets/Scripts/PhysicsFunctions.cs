using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhysicsFunctions
{
    public static float GravityScale(float targetGravity, float originalGravity)
    {
        return targetGravity / originalGravity;
    }

    public static void FindArrowVelocity(Vector3 source, Vector3 target, float minSpeed, float maxSpeed,
        float gravityMagnitude,
        Action<float, Vector3> onArrowVelocityFound, Action<float> onCannotFindVelocity)
    {
        
        //var speed = minSpeed;
        var lerp = 0f;

        while (lerp <= 1f)
        {
            var speed = Mathf.Lerp(minSpeed, maxSpeed, lerp);
            lerp += 0.1f;

            var results = Fts.solve_ballistic_arc(source, speed, target,
                gravityMagnitude, out Vector3 result0, out Vector3 result1);

            if (results <= 0)
            {
                speed += 1;
                continue;
            }

            onArrowVelocityFound?.Invoke(speed, result0);
            return;
        }

        onCannotFindVelocity?.Invoke(maxSpeed);

    }

    public static void FindArrowVelocityLateral(Vector3 source,
        Vector3 target,
        float lateralSpeed,
        float maxHeight,
        Action<float, Vector3, float> onArrowVelocityFound,
        Action<float> onCannotFindVelocity)
    {
        
        if (maxHeight < target.y)
        {
            maxHeight = target.y;
        } 

        Debug.DrawLine(new Vector2(-10, maxHeight), new Vector2(10, maxHeight), Color.yellow, 1);
        
        var foundArc = Fts.solve_ballistic_arc_lateral(source, lateralSpeed, target,
            maxHeight, out Vector3 force, out var gravity);

        if (!foundArc)
        {
            onCannotFindVelocity?.Invoke(lateralSpeed);
            return;
        }

        onArrowVelocityFound?.Invoke(lateralSpeed, force, gravity);
    }
    /*
    public static float Distance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(a, b);
    }

    public static float Distance(Vector2 a, Vector3 b)
    {
        return Vector2.Distance(a, b);
    }
    */

    public static float Distance(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }

    public static float Distance(Transform t1, Transform t2)
    {
        return Vector2.Distance(t1.position, t2.position);
    }

    public static double Distance(GameObject g1, GameObject g2)
    {
        return Vector2.Distance(g1.transform.position, g2.transform.position);
    }

    public static double LateralDistance(GameObject g1, GameObject g2)
    {
        return LateralDistance(g1.transform.position.x, g2.transform.position.x);
    }

    public static double LateralDistance(float x1, float x2)
    {
        return Mathf.Abs(x1 - x2);
    }

    public static double LateralDistance(Vector3 p1, Vector2 p2)
    {
        return LateralDistance(p1.x, p2.x);
    }
}