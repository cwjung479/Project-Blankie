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
    public TutorialManager manager;

    void Start()
    {
        flagRenderer = flag.GetComponent<SpriteRenderer>();
		//bubbleRenderer = flag.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("colliaon");
        if (col.tag == "flag")
        {
            print("yo");
            flagRenderer.sprite = victoryFlag;
            manager.completedTutorial();
            //bubbleRenderer.sprite = dreamBubble;
        }
    }
}
