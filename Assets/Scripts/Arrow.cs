using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour {

    
    private Rigidbody2D _rigidBody;
    private int _damage;
    private List<EntityTag> _targetEntityTags;
    private int _shooterId;

    public void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();


    }

    public void Initialize(Vector2 force, float targetGravity, int damage, List<EntityTag> targetEntityTags, int shooterId)
    {

        _targetEntityTags = targetEntityTags;
        _shooterId = shooterId;

        try
        {
            _rigidBody.gravityScale = PhysicsFunctions.GravityScale(targetGravity, Physics.gravity.magnitude);

            _rigidBody.AddForce(force * _rigidBody.mass, ForceMode2D.Impulse);


        }
        catch{}

        //_rigidbody.AddForce(force);

        _damage = damage;

        GameObject.Destroy(gameObject, 5);

        /*
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>()) {
            renderer.color = color;
        }
        */
    }

    public void FixedUpdate() {
        Vector2 v = _rigidBody.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Floor")
        {
            Destroy(gameObject);
        }

        if (other.GetInstanceID() == _shooterId)
        {
            return;
        }

        var otherEntity = other.GetComponent<Entity>();
        if (otherEntity == null)
        {
            return;
        }

        if (otherEntity.CanBeTargetedBy(_targetEntityTags))
        {
            otherEntity.DoDamage(_damage);
            Destroy(gameObject);
        }
        

    }

   
}