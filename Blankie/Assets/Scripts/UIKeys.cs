using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeys : MonoBehaviour
{
    public Image w;
    public Image a;
    public Image s;
    public Image d;
    public Image space;
    public Image left;
    public Image right;
    private int zone;

    public Image h1;
    public Image h2;
    public Image h3;
    public Image h4;
    public Image h5;
    public int lives = 10;
    public Text motivation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        highlightKeys();
        
    }

    public void lostLife()
    {
        lives--;
        print("hello: "+lives);
        if (lives == 8)
        {
            h5.color = Color.black;
        }else if (lives == 6)
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
        else if(lives ==0)
        {
            h1.color = Color.black;
            motivation.text = "It's okay! Just keep trying!\n This is only the tutorial!";
        }
    }

    public void activeZone(int zone)
    {
        this.zone = zone;
    }

    private void highlightKeys()
    {
        if(zone == 0)
        {
            w.color = Color.white;
            a.color = Color.yellow;
            s.color = Color.white;
            d.color = Color.yellow;
            space.color = Color.white;
            left.color = Color.white;
            right.color = Color.white;
        }else if (zone ==1 |zone == 2 || zone ==3)
        {
            w.color = Color.white;
            a.color = Color.white;
            s.color = Color.white;
            d.color = Color.white;
            space.color = Color.yellow;
            left.color = Color.white;
            right.color = Color.white;
        }
        else if (zone == 4)
        {
            w.color = Color.white;
            a.color = Color.white;
            s.color = Color.yellow;
            d.color = Color.white;
            space.color = Color.white;
            left.color = Color.white;
            right.color = Color.white;
        }
        else if (zone == 5 || zone ==6)
        {
            w.color = Color.white;
            a.color = Color.white;
            s.color = Color.white;
            d.color = Color.white;
            space.color = Color.white;
            left.color = Color.white;
            right.color = Color.yellow;
        }
        else if (zone == 7 | zone == 8 || zone == 9)
        {
            w.color = Color.white;
            a.color = Color.white;
            s.color = Color.white;
            d.color = Color.white;
            space.color = Color.white;
            left.color = Color.yellow;
            right.color = Color.white;
        }
        else
        {
            w.color = Color.white;
            a.color = Color.white;
            s.color = Color.white;
            d.color = Color.white;
            space.color = Color.white;
            left.color = Color.white;
            right.color = Color.white;
        }
        
    }
}
