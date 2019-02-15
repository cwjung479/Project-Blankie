using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    //Animation 
    private Animator anim;

    //Speed values (Make sure run and temp are the same)
    public float runSpeed = 40f;
    private float tempSpeed = 40f;
    private float defaultGravity = 3f;

    private float horizontalMove = 0f;
    private float timePressed = 0f;
    private Rigidbody2D rb2d;
    private GameControl gamecontroller;

    private GameObject pauseMenu;

    void Start()
    {
        gamecontroller = GameControl.instance;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Dynamic;

        pauseMenu = GameObject.Find("/Canvas");
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //While in the air
        if (!gamecontroller.grounded)
        {
            runSpeed = tempSpeed / 2f;
        }
        else {
            gamecontroller.letGo = false;
            runSpeed = tempSpeed;
            rb2d.gravityScale = defaultGravity;
        }

        if (Input.GetButtonDown("Jump") && !gamecontroller.grappling && !gamecontroller.colliding && GameControl.instance.grounded)
        {
            gamecontroller.jump = true;
            gamecontroller.crouch = false;

        }
        else if (Input.GetButtonUp("Jump"))
        {
            gamecontroller.jump = false;
            rb2d.gravityScale = defaultGravity;
        }


        //Jump and Float
        if (Input.GetButton("Jump") && !gamecontroller.grappling && !gamecontroller.colliding)
        {
            if (rb2d.velocity.y < -0.1f && !gamecontroller.grappling)
            {
                gamecontroller.crouch = false;
                gamecontroller.jump = false;
                gamecontroller.floating = true;
                rb2d.gravityScale = defaultGravity / 9f;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            gamecontroller.floating = false;
            rb2d.gravityScale = defaultGravity;
        }

        //Crouching
        if (Input.GetButtonDown("Crouch") && gamecontroller.grounded)
        {
            gamecontroller.crouch = true;
            gamecontroller.jump = false;
            gamecontroller.floating = false;
        }
        else if (Input.GetButtonUp("Crouch"))
            gamecontroller.crouch = false;

        //Grappling
        if (Input.GetMouseButtonDown(0))
        {
            // Do nothing if the pause menu is open
            if (!pauseMenu.activeSelf)
            {
                controller.shootRope();
                gamecontroller.crouch = false;
                gamecontroller.jump = false;
                gamecontroller.floating = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            controller.releaseRope();
            gamecontroller.grappling = false;
        }

        //Animations
        if (gamecontroller.crouch)
            anim.SetTrigger("crawl");
        else if (gamecontroller.jump)
            anim.SetTrigger("jump");
        else if (gamecontroller.floating)
            anim.SetTrigger("floatInAir");
        else if (gamecontroller.whipping)
            anim.SetTrigger("whipping");
        else if (gamecontroller.grappling)
            anim.SetTrigger("grapple");
        else
            anim.SetTrigger("run");
    }

    private void FixedUpdate(){
        // Move Character
        controller.Move(horizontalMove * Time.fixedDeltaTime);
        gamecontroller.jump = false;
    }
}
