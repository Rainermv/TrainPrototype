using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float SPEED;
    public int HEALTH;

    public HealthBar HealthBar;

    [HideInInspector]
    public string OpponentTag;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;


    private int maxHealth;
    private float directionMultiplier = 1;

    public bool isPlayer;

    public Color _originalColor { get; set; }

    private void Awake() {

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;

        maxHealth = HEALTH;

    }

    private void Start()
    {
        Initialize(isPlayer? Tags.PLAYER: Tags.ENEMY);
    }

    // Start is called before the first frame update
    public void Initialize(string setTag)
    {
        var renderer = GetComponent<SpriteRenderer>();

        tag = setTag;

        if (setTag == Tags.PLAYER) {
            OpponentTag = Tags.ENEMY;
        }

        if (setTag == Tags.ENEMY) {
            OpponentTag = Tags.PLAYER;
        }

        
    }

   

    // Update is called once per frame
    void Update()
    {
        if (HEALTH < 0) {
            Die();
            return;
        }

        if (_rigidbody2D == null)
        {
            return;
        }


        _rigidbody2D.velocity = new Vector2(SPEED, 0);

        /*
        if (Target == null) {
            _rigidbody2D.velocity = new Vector2(SPEED * directionMultiplier, 0);
            return;
        }

        if (Target.active == false) {
            Target = null;
            return;
        }

        if (Vector2.Distance(Target.transform.position, transform.position) > RANGE) {
            Target = null;
            return;
        }
               

        _rigidbody2D.velocity = new Vector2(0, 0);
        */


    }

    private void Die() {

        gameObject.SetActive(false);
        Destroy(gameObject);

    }



    public void MessageDamage(int damage) {
        HEALTH -= damage;

        _spriteRenderer.color = Color.Lerp(Color.black, _originalColor, (float)HEALTH / (float)maxHealth);

    }

    
}
