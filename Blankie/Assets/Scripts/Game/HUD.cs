using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{

    public Image h1;
    public Image h2;
    public Image h3;
    public Image h4;
    public Image h5;
    public int lives = 10;
    private int zone;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activeZone(int zone) {
        this.zone = zone;
    }

    public void lostLife()
    {
        lives--;
        if (lives == 4)
        {
            h5.color = Color.black;
        }
        else if (lives == 3)
        {
            h4.color = Color.black;
        }
        else if (lives == 2)
        {
            h3.color = Color.black;
        }
        else if (lives == 1)
        {
            h2.color = Color.black;
        }
        else if (lives == 0)
        {
            h1.color = Color.black;
        }
    }

    public void newLife()
    {
        lives++;
        if (lives == 8)
        {
            h5.color = Color.black;
        }
        else if (lives == 6)
        {
            h4.color = Color.black;
        }
        else if (lives == 4)
        {
            h3.color = Color.black;
        }
        else if (lives == 2)
        {
            h2.color = Color.black;
        }
        else if (lives == 0)
        {
            h1.color = Color.black;
        }
    }
}
