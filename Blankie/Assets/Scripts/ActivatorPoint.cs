using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorPoint : MonoBehaviour
{
    public int ID;
    public bool isDoorSwitch;
    public bool isEnemySwitch;
    public bool isCamSwitch;

    public bool activator;
    public GameObject linkedDoor;
    public bool requiresKey;

    public GameObject linkedEnemy;

    public Cinemachine.CinemachineVirtualCamera cam1;
    public Cinemachine.CinemachineVirtualCamera cam2;

    private bool active;

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
        activate(collision.gameObject.GetComponent<CharacterController2D>());
    }

    // Cristian Rangel
    //
    // Activate the point's functionality
    public void activate(CharacterController2D player) {
        if (active) {
            if (isDoorSwitch && (!requiresKey || player.hasKey)) {
                linkedDoor.SetActive(!activator);
            }
            if (isEnemySwitch) {
                linkedEnemy.SetActive(!linkedEnemy.activeSelf);
            }
        }
        if (isCamSwitch) {
            if (cam1.Priority > cam2.Priority) {
                cam1.Priority = 10;
                cam2.Priority = 11;
            } else {
                cam1.Priority = 11;
                cam2.Priority = 10;
            }
        }
    }

    // Cristian Rangel
    //
    // Deactivate this point, and make sure the linked object is inactive
    public void deactivate()
    {
        active = false;
        if (isDoorSwitch) {
            linkedDoor.SetActive(false);
        }
        if (isEnemySwitch) {
            linkedEnemy.SetActive(false);
        }
    }
}
