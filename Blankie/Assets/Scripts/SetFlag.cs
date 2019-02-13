using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFlag : MonoBehaviour
{
    public GameObject flag;
    public Sprite victoryFlag;
    public Sprite dreamBubble;
    public SpriteRenderer flagRenderer;
    public SpriteRenderer bubbleRenderer;

    void Start()
    {
        flagRenderer = flag.GetComponent<SpriteRenderer>();
		//bubbleRenderer = flag.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "flag")
        {
            flagRenderer.sprite = victoryFlag;
            //bubbleRenderer.sprite = dreamBubble;
        }
    }
}
