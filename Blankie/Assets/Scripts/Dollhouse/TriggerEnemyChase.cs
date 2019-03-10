using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemyChase : MonoBehaviour
{
    public TargetingEnemyMovement tem;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            tem.setChase(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            tem.setChase(false);
    }
}
