using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    //Text Objects
    public static GameControl instance;
    public GameObject gameOverText;
    public GameObject winText;
    public GameObject pauseScreen;

    //Player States
    public bool grounded = false;
    public bool running = false;
    public bool floating = false;
    public bool jump = false;
    public bool crouch = false;
    public bool grappling = false;
    public bool letGo = false;
    public bool colliding = false;
    public bool whipping = false;

    //Camera Controls
    public float cameraX = 5f;
    public float cameraY = 5f;

    //Game States
    public bool gameOver = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update(){
        if (gameOver == true && Input.GetButton("Fire2")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            pauseScreen.SetActive(!pauseScreen.activeSelf);
    }

    public void PlayerDied(){
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void PlayerWin()
    {
        winText.SetActive(true);
        gameOver = true;
    }
}
