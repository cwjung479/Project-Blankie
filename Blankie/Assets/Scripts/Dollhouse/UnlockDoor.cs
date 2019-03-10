using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject door;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            door.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
