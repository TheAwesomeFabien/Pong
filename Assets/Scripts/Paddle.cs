using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Paddle : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    public bool isAI;
    private Ball ball;
    private BoxCollider2D col;

    private float randomYOffset;

    private Vector2 forwardDirection;
    private bool firstIncoming;
    
    public enum Side { Left, Right }

    [SerializeField] private Side side;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        col = GetComponent<BoxCollider2D>();
        if (side == Side.Left)
        {
            forwardDirection = Vector2.right;
            
        } else if (side == Side.Right) 
        {
            forwardDirection = Vector2.left; 
        }
    }

    private void Update()
    {
        float targetYPosition = GetNewYPostion();
        
        ClampPosition(ref targetYPosition);

        transform.position = new Vector3(transform.position.x, targetYPosition, transform.position.z);
    }

    private void ClampPosition(ref float yPosition)
    {
        float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y;
        float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;

        yPosition = Mathf.Clamp(yPosition, minY, maxY);
    }

    private float GetNewYPostion()
    {
        float result = transform.position.y;
        
        if (isAI)
        {
            if (BallIncoming())
            {
                if (firstIncoming)
                {
                    print("First time incoming");
                    firstIncoming = false;
                    randomYOffset = GetRandomOffset();
                }

                result = Mathf.MoveTowards(transform.position.y, ball.transform.position.y + randomYOffset, moveSpeed * Time.deltaTime);
            }
            else
            {
                firstIncoming = true;
            }
        }
        else
        {
            float movement = Input.GetAxisRaw("Vertical")* moveSpeed * Time.deltaTime;
            result = transform.position.y + movement;
        }

        return result;
    }

    private bool BallIncoming()
    {
        float dotP = Vector2.Dot(ball.velocity, forwardDirection);
        return dotP < 0f;
    }

    private float GetRandomOffset()
    {
        float maxOffset = col.bounds.extents.y;
        return Random.Range(-maxOffset, maxOffset);
    }
}
