using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{

    public PickUpCoin coinPrefab;
    PickUpCoin coin = null;
    Vector2 recentDeath = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("Enemy"))
        {
            recentDeath = other.transform.position;
            other.gameObject.SetActive(false);
            createPickUp();
        }

    }

    public void createPickUp()
    {
        coin = Instantiate(coinPrefab);
        coin.transform.position = recentDeath;
    }
}
