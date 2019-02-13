using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpCoin : MonoBehaviour
{

    public Text scoreText;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.FindWithTag("Score").GetComponent<Text>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Time.deltaTime * 50, 0, Time.deltaTime * 25);
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            score++;
            scoreText.text = "Score: "+score.ToString();
        }

    }
}