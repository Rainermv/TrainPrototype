using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class keyboardMove : MonoBehaviour
{
    public float SPEED;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left * SPEED);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right * SPEED);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.up * SPEED);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.down * SPEED);
    }

    
}
