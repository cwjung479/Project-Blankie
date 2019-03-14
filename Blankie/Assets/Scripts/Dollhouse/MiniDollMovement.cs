using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDollMovement : MonoBehaviour
{
    public float speed;

    private int direction = -1;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb2d.transform.position.x < 129)
            Destroy(gameObject);
        else
            move();
    }

    void move()
    {
        Vector2 movement = new Vector2(direction, 0.0f);

        rb2d.transform.Translate(movement * Time.deltaTime * speed);
    }
}
