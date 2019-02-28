using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public GameObject player;
    public GameObject doneScreen;

    // Start is called before the first frame update
    void Start()
    {
        //doneScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void completedTutorial()
    {
        player.SetActive(false);
        doneScreen.SetActive(true);
    }

}
