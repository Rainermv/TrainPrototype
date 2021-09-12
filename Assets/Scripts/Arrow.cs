using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour {

    
    private Rigidbody2D _rigidbody;
    private string _bowTag;
    private int _damage;

    public void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 force, float targetGravity, int damage, string tag, Color color) {

        Debug.Log(force);

        GetComponent<SpriteRenderer>().color = color;

        try
        {
            _rigidbody.gravityScale = PhysicsFunctions.GravityScale(targetGravity, Physics.gravity.magnitude);

            _rigidbody.AddForce(force * _rigidbody.mass, ForceMode2D.Impulse);


        }
        catch{}

        //_rigidbody.AddForce(force);
        _bowTag = tag;

        _damage = damage;

        GameObject.Destroy(gameObject, 5);

        /*
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>()) {
            renderer.color = color;
        }
        */
    }

    public void FixedUpdate() {
        Vector2 v = _rigidbody.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.tag != _bowTag) {
            //_rigidbody.velocity = Vector2.zero;

            //this.enabled = false;
            //_rigidbody.isKinematic = true;
            //this.transform.parent = other.transform;

            if (other.tag != "Untagged") {
                other.SendMessage("MessageDamage", _damage);
                GameObject.Destroy(gameObject);
            }

            
        }
    }
}