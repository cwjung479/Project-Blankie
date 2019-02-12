using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Vector2 destination;
    public float speed = 1;
    public float distance = 2;
    public GameObject nodePrefab;
    public GameObject player;
    public GameObject lastNode;
    bool done = false;

    int vertexCount = 2;

    public List<GameObject> Nodes = new List<GameObject>();

    // Use this for initialization
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        lastNode = transform.gameObject;
    }

    // Update is called once per frame
    void Update(){
        transform.position = Vector2.MoveTowards(transform.position, destination, speed);

        if ((Vector2)transform.position != destination){
            if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance){
                CreateNode();
            }
        }
        else if (done == false)
        {
            done = true;

            while (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
                CreateNode();
            
            lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }
    }


    void CreateNode()
    {
        Vector2 pos2Create = player.transform.position - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= distance;
        pos2Create += (Vector2)lastNode.transform.position;

        GameObject go = (GameObject)Instantiate(nodePrefab, pos2Create, Quaternion.identity);
        
        go.transform.SetParent(transform);
        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
        lastNode = go;

        Nodes.Add(lastNode);

        vertexCount++;
    }
}
