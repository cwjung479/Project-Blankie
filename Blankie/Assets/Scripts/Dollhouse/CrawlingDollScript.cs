using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Nate Trank
public class CrawlingDollScript : MonoBehaviour
{
    public float speed;
    public GameObject player;
    public float dollDetectDistance;
    public float crawlingGrabRange;
    public PlayerMovement ps;

    // 1 for to the left, -1 for to the right
    private int direction;
    private Rigidbody2D rb2d;
    private float distance;
    private float playerSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = 0;
        playerSpeed = ps.runSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shouldMove() && dollLineOfSight())
        {
            Vector2 movement = new Vector2(direction, 0.0f);
            rb2d.transform.Translate(movement * Time.deltaTime * speed);
        }
    }

    bool dollLineOfSight()
    {
        Vector2 start = transform.position;
        Vector2 direction = player.transform.position - transform.position;
        float distance = dollDetectDistance;

        RaycastHit2D LOS = Physics2D.Raycast(start, direction, distance);
        if (LOS.collider.tag.Equals("Ground") ||
            LOS.collider.tag.Equals("Wall"))
            return false;

        return true;
    }

    bool shouldMove()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        float verticalDistance = transform.position.y - player.transform.position.y;

        shouldSlow(distance, verticalDistance);

        if (distance < dollDetectDistance && verticalDistance <= 2 && verticalDistance >= -2)
        {
            findDirection();
            return true;
        }

        return false;
    }

    void shouldSlow(float distance, float verticalDistance)
    {
        if (distance <= crawlingGrabRange && verticalDistance <= 2 && verticalDistance >= -2)
            ps.runSpeed = playerSpeed / 4;
        else
            ps.runSpeed = playerSpeed;
        
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

    

}
