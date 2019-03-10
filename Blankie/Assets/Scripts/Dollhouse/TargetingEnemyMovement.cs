using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Nate Trank
public class TargetingEnemyMovement : MonoBehaviour
{
    public float chaseSpeed;
    public float neutralSpeed;
    public GameObject player;
    public float yDetectDistance;

    public GameObject Boundary;
    public Transform neutralPositionObject;

    private Rigidbody2D rb2d;
    private bool chasing;
    
    // 0 for still, 1 for right, -1 for left
    private int direction;
    private float speed;

    private float neutralPosition;

    void Start()
    {
        chasing = false;
        rb2d = GetComponent<Rigidbody2D>();
        direction = 0;
        neutralPosition = neutralPositionObject.position.x;
    }

    void FixedUpdate()
    {
        findDirection();

        Vector2 movement = new Vector2(direction, 0.0f);

        rb2d.transform.Translate(movement * Time.deltaTime * speed);
    }

    public void setChase(bool set)
    {
        chasing = set;
    }
    

    void findDirection()
    {
        if (chasing)
        {
            speed = chaseSpeed;

            if (player.transform.position.x < rb2d.transform.position.x &&
                player.transform.position.y - rb2d.transform.position.y < yDetectDistance)
                direction = -1;
            else if (player.transform.position.x > rb2d.transform.position.x &&
                     player.transform.position.y - rb2d.transform.position.y < yDetectDistance)
                direction = 1;
            else
                direction = 0;
        }
        else
        {
            speed = neutralSpeed;

            if (rb2d.transform.position.x - neutralPosition > 0.3)
                direction = -1;
            else if (rb2d.transform.position.x - neutralPosition < -0.3)
                direction = 1;
            else
                direction = 0;
        }
        
    }

}
