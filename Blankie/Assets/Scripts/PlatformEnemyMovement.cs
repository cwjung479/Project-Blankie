using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemyMovement : MonoBehaviour
{
    // script for an enemy moving back and forth on a platform
    // enemy is bounded by child gameObjects

    // movement speed of enemy
    public float speed;
    
    // movement boundaries for enemy
    public Transform leftBoundaryObject;
    public Transform rightBoundaryObject;

    // -1 for to the left, 1 for to the right
    private int direction;
    private Rigidbody2D rb2d;

    private float leftBoundary;
    private float rightBoundary;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = 1;
        Debug.Log(leftBoundaryObject.position.x + " " + rightBoundaryObject.position.x);
        leftBoundary = leftBoundaryObject.position.x;
        rightBoundary = rightBoundaryObject.position.x;
    }

    void FixedUpdate()
    {
        findDirection();
        Vector2 movement = new Vector2(direction, 0.0f);
        
        rb2d.transform.Translate(movement * Time.deltaTime * speed);
    }

    void findDirection()
    {
        // if enemy is at the right boundary, turn left
        if (rb2d.transform.position.x >= rightBoundary)
            direction = -1;

        // if enemy is at the left boundary, turn right
        else if (rb2d.transform.position.x <= leftBoundary)
            direction = 1;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            Destroy(col.gameObject, 0);
    }

}
