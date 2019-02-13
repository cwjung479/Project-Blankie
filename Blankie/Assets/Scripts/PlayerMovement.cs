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

    //grapple values
    private LineRenderer ropeRenderer;
    private DistanceJoint2D dj;
    public float maxDist = 5f;
    public LayerMask grappleMask;

    public GameObject whip;
    private Collider2D whipCollider;

    void Start()
    {
        gamecontroller = GameControl.instance;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Dynamic;

        dj = GetComponent<DistanceJoint2D>();
        ropeRenderer = GetComponent<LineRenderer>();
        dj.enabled = false;

        whip = GameObject.FindWithTag("Whip"); //gets whip object
        whipCollider = whip.GetComponent<Collider2D>();
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
            shootRope();
            gamecontroller.crouch = false;
            gamecontroller.jump = false;
            gamecontroller.floating = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            releaseRope();
            gamecontroller.grappling = false;
        }

		//Whipping
		if(Input.GetMouseButtonDown(1)){
            gamecontroller.whipping = true;
            StartCoroutine(whipActive());
            
        }
        if (!gamecontroller.whipping)
        {
            whipCollider.enabled = false;
        }

        //Animations
        if (gamecontroller.crouch)
            anim.SetTrigger("crawl");
        else if (gamecontroller.jump)
            anim.SetTrigger("jump");
        else if (gamecontroller.floating)
            anim.SetTrigger("floatInAir");
        else if (gamecontroller.whipping)
        {
            anim.SetTrigger("whipping");
        }
        else
            anim.SetTrigger("run");
    }

    private IEnumerator whipActive()
    {
        whipCollider.enabled = true;
        yield return new WaitForSeconds(.5f);
        gamecontroller.whipping = false;
    }

    private void FixedUpdate(){
        // Move Character
        controller.Move(horizontalMove * Time.fixedDeltaTime);
        gamecontroller.jump = false;

        if (gamecontroller.grappling)
            updateRopePositions(ropeRenderer.GetPosition(1));
    }

    void shootRope()
    {
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var angle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        var aimDirection = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, maxDist + 1, grappleMask);

        if (hit.collider != null)
        {
            gamecontroller.grappling = true;
            Debug.Log("Hit something");
            anim.SetTrigger("grapple");
            ropeRenderer.enabled = true;
            ropeRenderer.positionCount = 2;
            updateRopePositions(hit.point);
            dj.enabled = true;
            dj.connectedAnchor = hit.point;
            dj.anchor = Vector2.zero;
            dj.distance = maxDist;
        }
        else
            Debug.Log("didnt hit");
    }

    void updateRopePositions(Vector3 position)
    {
        float xDist = position.x - transform.position.x;
        float yDist = position.y - transform.position.y;

        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, position);
    }

    void releaseRope()
    {
        gamecontroller.letGo = true;
        ropeRenderer.enabled = false;
        dj.enabled = false;
    }
}
