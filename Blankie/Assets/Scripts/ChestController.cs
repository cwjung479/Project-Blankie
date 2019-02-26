using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public PickUpCoin toSpawn;
    public string toTag;
    private bool active;

    void Start () {
        active = true;
    }

    public void wasHit() {
        if (active) {
            toSpawn = Instantiate(toSpawn);
            toSpawn.transform.position = new Vector2(transform.position.x - 0.75f, transform.position.y + 0.75f);
            toSpawn.tag = toTag;
            transform.GetChild(1).gameObject.transform.Rotate(0, 0, -45);
            active = false;
        }
    }
}
