using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int HEALTH;
    private int MAX_HEALTH;

    public HealthBar HealthBar;

    private void Awake() {
        MAX_HEALTH = HEALTH;
    }

    private void Update() {
        
        if (HEALTH < 0) {
            Die();
        }

        

    }

    private void Die() {

        Destroy(gameObject);

    }

    public void MessageDamage(int damage) {
        HEALTH -= damage;

        if (HealthBar != null) {

            //Debug.Log($"{HEALTH} / {MAX_HEALTH}");
            HealthBar.UpdateValue((float)HEALTH, (float)MAX_HEALTH);

        }
    }
}
