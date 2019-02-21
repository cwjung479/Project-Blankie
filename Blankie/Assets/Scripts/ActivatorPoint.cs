using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorPoint : MonoBehaviour
{
    public int ID;
    public bool activator;
    public GameObject linkedDoor;

    public bool active;

    void Start()
    {
        active = true;
    }

    // Cristian Rangel
    //
    // If the player touches this, open or close linked door as necessary
    //   based on whether this is deemed an activator or deactivator
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
            linkedDoor.SetActive(!activator);
    }

    public void deactivate()
    {
        active = false;
        linkedDoor.SetActive(false);
    }
}
