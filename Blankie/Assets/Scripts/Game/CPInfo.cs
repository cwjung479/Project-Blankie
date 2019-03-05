using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPInfo : MonoBehaviour
{
    public int ID;
    public Canvas HUD;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("Player"))
        {
            if(HUD == null)
            {
                print("ugh");
            }
            HUD.GetComponent<HUD>().activeZone(ID);
        }

    }
}
