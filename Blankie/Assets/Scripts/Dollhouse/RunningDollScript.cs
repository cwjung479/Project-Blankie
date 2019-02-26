using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Nate Trank
public class RunningDollScript : MonoBehaviour
{
    public float speed;
    public float lungeSpeed;
    public GameObject player;
    public float dollDetectDistance;
    public float runningLungeRange;

    // 1 for to the left, -1 for to the right
    private int direction;
    private Rigidbody2D rb2d;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        float verticalDistance = transform.position.y - player.transform.position.y;
        findDirection();

        if (dollLineOfSight())
        {
            if (distance <= runningLungeRange && verticalDistance <= 2 && verticalDistance >= -2)
                lunge();

            else if (distance <= dollDetectDistance && verticalDistance <= 2 && verticalDistance >= -2)
                move();
        }
    }

    bool dollLineOfSight()
    {
        RaycastHit hit;

        if (Physics.Linecast(transform.position, player.transform.position, out hit))
        {
            if (hit.transform.tag == "Ground" ||
                hit.transform.tag == "Wall")
                return false;
        }

        return true;
    }

    void move()
    {
        // move the object in the direction of the player
        Vector2 movement = new Vector2(direction, 0.0f);
        rb2d.transform.Translate(movement * Time.deltaTime * speed);
    }

    void lunge()
    {
        // if close enough, lunge at the player
        Vector2 movement = new Vector2(direction, 0.0f);
        rb2d.transform.Translate(movement * Time.deltaTime * lungeSpeed);
    }

    void findDirection()
    {
        if (player.transform.position.x - transform.position.x < 0)
            direction = -1;
        else if (player.transform.position.x - transform.position.x > 0)
            direction = 1;
        else
            direction = 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            Destroy(gameObject, 0);
    }
}
