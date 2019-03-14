using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// nate trank
public class BossString : MonoBehaviour
{
    public float speed;
    public float attackSpeed;
    public float raiseSpeed;
    public float dropDistance;

    public float trackTime;
    public float waitTime;

    public float xTriggerPosition = 128f;
    public GameObject player;

    private int direction;
    private Rigidbody2D rb2d;
    private float startingY;

    private bool tracking;
    private bool attacking;
    private bool raising;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        direction = 0;
        rb2d = GetComponent<Rigidbody2D>();
        startingY = rb2d.transform.position.y;
        tracking = false;
        attacking = false;
        raising = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if the player just entered the area
        if (player.transform.position.x > xTriggerPosition 
            && !attacking && !tracking && !raising)
        {
            tracking = true;
            time = Time.fixedTime;
        }
        else if (player.transform.position.x < xTriggerPosition
                 && (attacking || tracking || raising))
        {
            tracking = false;
        }

        if (tracking)
        {
            // while tracking the player
            if ((Time.fixedTime - time) < trackTime)
            {
                findDirection();
                move();
            }
            // stop tracking and wait to attack
            else
            {
                tracking = false;
                attacking = true;
                time = Time.fixedTime;
            }
        }

        // attack after waiting
        if (attacking && (Time.fixedTime - time) > waitTime)
        {
            if (rb2d.transform.position.y > startingY - dropDistance)
            {
                drop();
            }
            else
            {
                attacking = false;
                raising = true;
            }
        }

        if (raising)
        {
            if (rb2d.transform.position.y < startingY)
            {
                raise();
            }
            else
            {
                raising = false;
                tracking = true;
                time = Time.fixedTime;
            }
        }

    }

    void findDirection()
    {
        if (player.transform.position.x - rb2d.transform.position.x < -0.3)
            direction = -1;
        else if (player.transform.position.x - rb2d.transform.position.x > 0.3)
            direction = 1;
        else
            direction = 0;
    }

    void move()
    {
        // move the string in the direction of the player
        Vector2 movement = new Vector2(direction, 0.0f);
        rb2d.transform.Translate(movement * Time.deltaTime * speed);
    }

    void drop()
    {
        // drop the string to attack
        Vector2 attack = new Vector2(0.0f, -1);
        rb2d.transform.Translate(attack * Time.deltaTime * attackSpeed);
    }

    void raise()
    {
        // raise the string back up after attacking
        Vector2 raise = new Vector2(0.0f, 1);
        rb2d.transform.Translate(raise * Time.deltaTime * raiseSpeed);
    }
}
