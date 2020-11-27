using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 velocity { get; private set; }

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float maxBounceAngle = 45f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity =Vector2.left * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Paddle")
        {
            BounceFromPaddle(collision.collider);   
        }
        else
        { 
            Bounce(); 
        }
        
    }

    private void Bounce()
    {
        velocity = new Vector2(velocity.x, -velocity.y);
    }

    private void BounceFromPaddle(Collider2D collider)
    {
        float colYExtent = collider.bounds.extents.y;
        float yOffset = transform.position.y - collider.transform.position.y;

        float yRatio = yOffset / colYExtent;
        float bounceAngle = maxBounceAngle * yRatio * Mathf.Deg2Rad;
        
        Vector2 bounceDirection = new Vector2(Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle));

        bounceDirection.x *= Mathf.Sign(-velocity.x);
        
        velocity = bounceDirection * moveSpeed;
    }
}
