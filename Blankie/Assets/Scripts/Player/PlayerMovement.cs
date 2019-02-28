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
    private float verticalMove = 0f;
    private float timePressed = 0f;
    private Rigidbody2D rb2d;
    private GameControl gamecontroller;

    private GameObject pauseMenu;

    void Start()
    {
        if(controller == null)
        {
            print("ugh");
        }
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
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed / 2;

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


        //Jump
        if (Input.GetButtonDown("Jump") &&
             !gamecontroller.grappling &&
             !gamecontroller.hittingCeiling &&
             (gamecontroller.grounded || gamecontroller.climbing))
        {
            gamecontroller.jump = true;
            gamecontroller.crouch = false;

        }
        else if (Input.GetButtonUp("Jump"))
        {
            gamecontroller.jump = false;
        }


        //Float
        if (Input.GetButton("Jump") && 
            !gamecontroller.grappling && 
            !gamecontroller.grounded)
        {
            if (rb2d.velocity.y < -0.1f)
            {
                gamecontroller.crouch = false;
                gamecontroller.jump = false;
                gamecontroller.floating = true;
                if (!gamecontroller.climbing)
                    rb2d.gravityScale = defaultGravity / 9f;
            }
        }
        else if (Input.GetButtonUp("Jump") || gamecontroller.grounded)
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
                gamecontroller.crouch = false;
                gamecontroller.jump = false;
                gamecontroller.floating = false;
                gamecontroller.clicking = true;
                controller.shootRope();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gamecontroller.grappling = false;
            gamecontroller.clicking = false;
            controller.releaseRope();
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
        if (!gamecontroller.climbing)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, 0);
        } else
        {
            rb2d.gravityScale = 0;
            controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
        }
        gamecontroller.jump = false;
    }

    // Cristian Rangel
    //
    // If player leaves the ladder, they stop climbing
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (rb2d.velocity.y > runSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, runSpeed / 2);
            }
            gamecontroller.climbing = false;
            rb2d.gravityScale = defaultGravity;
        }
    }
}
