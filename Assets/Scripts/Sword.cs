using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Sword : MonoBehaviour
{
    public int DAMAGE;
    public float COOLDOWN;

    private Entity _entity;

    private void Awake() {
        _entity = GetComponent<Entity>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack() {
        while (true) {

            /*
            if (_entity.Target != null) {

                _entity.Target.SendMessage("MessageDamage", DAMAGE);

            }
            */

            yield return new WaitForSeconds(COOLDOWN);
        }

    }
}
