using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject hook;
    public bool ropeActive = false;

    private GameObject curHook;
    public PlayerMovement playerMovement;

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider.tag == "grappleObject")
            {
                ropeActive = true;
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<CharacterController2D>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                player.SendMessage("isGrappling", true);
            }
                
            if (ropeActive)
            {
                Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);
                curHook.GetComponent<RopeScript>().destination = destination;
            }
        }
        else if((Input.GetMouseButtonUp(0))){
            Destroy(curHook);
            ropeActive = false;
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<CharacterController2D>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.SendMessage("isGrappling", false);
        }
    }
}
