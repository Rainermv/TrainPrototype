using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Vector3 HealthBarInitialScale;

    public SpriteRenderer GreenBar;
    public SpriteRenderer RedBar;

    public void Awake() {

        GreenBar.enabled = false;
        RedBar.enabled = false;
    }

    
    public void UpdateValue(float value, float maxValue) {
        GreenBar.enabled = true;
        RedBar.enabled = true;


        // Size 0   = pos -0.5
        // Size 0.2 = pos -0.4
        // Size 0.4 = pos -0.3
        // Size 0.5 = pos -0.25
        // Size 0.6 = pos -0.2
        // Size 0.8 = pos -0.1
        // Size 1 = pos 0

        var size = value / maxValue;
        var pos = -(1 - size) / 2;

        GreenBar.transform.localPosition = new Vector3(pos, 0, 0);
        GreenBar.transform.localScale = new Vector3(size, 1, 1);

    }
}
