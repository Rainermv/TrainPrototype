using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PhysicsTest
{
    [TestCase(0,1,1)]
    [TestCase(1, 0, 1)]
    [TestCase(-1, 0, 1)]
    [TestCase(0, -1, 1)]
    [TestCase(-1, 1, 2)]

    public void LinearDistanceTest(float y1, float y2, float expectedDistance)
    {
        Assert.AreEqual(expectedDistance, PhysicsFunctions.LateralDistance(y1,y2));
    }

    [TestCase(0, 0, 0, 1, 1, 2)] 
    [TestCase(0, 0, 0, 1, 10, 20)]
    [TestCase(0, 0, 10, 10, 10, 20)]
    public void ArrowVelocityTest(float x1, float y1, float x2, float y2, int minSpeed, int maxSpeed, bool isNumber = true)
    {
        var gravity = 9.82f;
        PhysicsFunctions.FindArrowVelocity(
            new Vector2(x1, y1),
            new Vector2(x2, y2),
            minSpeed,
            maxSpeed,
            gravity,
            (speed, vector3) => Debug.Log($"Found target with speed: {speed}\n Velocity: {vector3}"),
            (speed) => Debug.Log($"Could not find target on speed: {speed}"));

    }

    [TestCase(0F, 0F, 10F, 10F, 10, 20, 5F)]
    public void ArrowVelocityLateralTest(float x1, float y1, float x2, float y2, int minSpeed, int maxSpeed,
        float maxHeight)
    {
        var gravity = 9.82f;
        PhysicsFunctions.FindArrowVelocityLateral(
            new Vector2(x1, y1),
            new Vector2(x2, y2),
            maxSpeed,
            maxHeight,
            (speed, vector3, gravity) => Debug.Log($"Found target with speed: [{speed}] and gravity [{gravity}]\n Velocity: {vector3}"),
            (speed) => Debug.Log($"Could not find target on speed: {speed}"));

    }

    [TestCase(1, 1)]
    [TestCase(10, 10)]
    [TestCase(1,10)]
    [TestCase(10, 1)]

    [TestCase(-1, -1)]
    [TestCase(-10,-10)]
    [TestCase(-1, -10)]
    [TestCase(-10,-1)]

    [TestCase(-1, 1)]
    [TestCase(-10, 10)]
    [TestCase(-1, 10)]
    [TestCase(-10, 1)]

    [TestCase(1, -1)]
    [TestCase(10, -10)]
    [TestCase(1, -10)]
    [TestCase(10, -1)]

    public void GravityScaleTest(float targetGravity, float originalGravity)
    {
        var gravityScale = PhysicsFunctions.GravityScale(targetGravity, originalGravity);
        Debug.Log($"targetGravity = {targetGravity}");
        Debug.Log($"originalGravity = {originalGravity}");
        Debug.Log($"gravityScale = {gravityScale}");
        Assert.AreEqual(gravityScale * originalGravity, targetGravity);
        Debug.Log(gravityScale);
    }

}
