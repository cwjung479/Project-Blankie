using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindDollMovement : MonoBehaviour
{
    public float speed;

    // 1 for to the left, -1 for to the right
    private int direction;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = 1;
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(direction, 0.0f);

        rb2d.transform.Translate(movement * Time.deltaTime * speed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
            direction = direction * -1;

        if (col.gameObject.CompareTag("Player"))
            Destroy(col.gameObject, 0);
    }

}
