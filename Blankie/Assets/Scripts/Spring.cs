using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.position = new Vector2(-150f, -250f);
    }
}
