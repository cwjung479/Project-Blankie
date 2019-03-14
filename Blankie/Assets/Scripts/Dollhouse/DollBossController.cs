using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollBossController : MonoBehaviour
{
    public int health;

    public GameObject door;
    public GameObject player;
    public float xDoorTrigger;

    private bool dead = false;

    void Update()
    {
        if (player.transform.position.x > xDoorTrigger)
            door.SetActive(true);
        else if (player.transform.position.x < xDoorTrigger - 5)
            door.SetActive(false);
    }

    public void takeDamage(int attackDamage)
    {
        health -= attackDamage;

        if (health <= 0)
        {
            dead = true;
        }
    }


}
