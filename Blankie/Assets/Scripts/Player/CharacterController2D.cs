using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject HUD;

    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] private Collider2D m_CrouchEnableCollider;				    // A collider that will be enabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    private Vector3 current_Velocity;
    private GameControl gamecontroller;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

    private int direction;

    // For particle effects
    public ParticleSystem landingParticles;
    public ParticleSystem invisiSparkles;

    // Checkpoint and door activator things
    // doorIDs keeps track of opener in 0 and closer in 1
    private GameObject lastCP;
    private ActivatorPoint[] lastActivators;
    public bool hasKey;

    // Blanket things: for whipping and swinging
    private LineRenderer ropeRenderer;
    private DistanceJoint2D dj;
    public float maxDist = 5f;
    [SerializeField] public LayerMask blanketMask;
    public int attackDamage = 2;

    // Necessary for generating the coin upon killing an enemy
    public PickUpCoin coinPrefab;
    PickUpCoin coin = null;

    // Chanye Jung, Isaac Hintergardt, Cristian Rangel
    private void Awake()
	{
        if (HUD == null)
        {
            print("UGH");
        }

        gamecontroller = GameControl.instance;
		m_Rigidbody2D = GetComponent<Rigidbody2D>();


        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
        
        dj = GetComponent<DistanceJoint2D>();
        ropeRenderer = GetComponent<LineRenderer>();
        dj.enabled = false;

        lastActivators = new ActivatorPoint[2];
        hasKey = false;
    }

    // Chanye Jung, Isaac Hintergardt, Cristian Rangel
	private void FixedUpdate()
	{
		bool wasGrounded = gamecontroller.grounded;
		gamecontroller.grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
                gamecontroller.grounded = true;
                if (!wasGrounded) {
                    landingParticles.Play();
                    OnLandEvent.Invoke();
                }
			}
		}

        if (gamecontroller.clicking)
            updateRopePositions(ropeRenderer.GetPosition(1));
    }

    // Chanye Jung, Isaac Hintergardt, Cristian Rangel
    //
    // Player movement logic
    public void Move(float xmove, float ymove)
    {
        // If crouching, check to see if the character can stand up
        if (!gamecontroller.crouch)
            checkCeilingCollider();

        if (!gamecontroller.crouch || gamecontroller.hittingCeiling)
            checkCeilingCollider();

        // Only control the player if grounded, airControl is turned on, or player is climbing
        if (gamecontroller.grounded || m_AirControl || gamecontroller.climbing)
        {

            // If crouching
            if (gamecontroller.crouch)
            {
                invisiSparkles.Play();
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                xmove *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null) {
                    m_CrouchDisableCollider.enabled = false;
                    m_CrouchEnableCollider.enabled = true;
                }
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null) {
                    m_CrouchDisableCollider.enabled = true;
                    m_CrouchEnableCollider.enabled = false;
                }

                invisiSparkles.Stop();

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            if (!gamecontroller.climbing) {
                if (!gamecontroller.grappling && !gamecontroller.letGo) {
                    // Move the character by finding the target velocity
                    Vector3 targetVelocity = new Vector2(xmove * 10f, m_Rigidbody2D.velocity.y);
                    // And then smoothing it out and applying it to the character
                    m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
                } else if (!gamecontroller.grappling) {
                    Vector3 targetVelocity = new Vector2(xmove * 10f, m_Rigidbody2D.velocity.y);
                    current_Velocity = m_Rigidbody2D.velocity;
                    m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref current_Velocity, m_MovementSmoothing);
                } else
                    m_Rigidbody2D.AddForce(new Vector2(xmove * 10, 0));
            } else {
                Vector3 targetVelocity = new Vector2(xmove * 10f, ymove * 10f);
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            }

            // If the input is moving the player right and the player is facing left...
            if (xmove > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (xmove < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if ((gamecontroller.grounded || gamecontroller.climbing) &&
             gamecontroller.jump &&
             !gamecontroller.hittingCeiling)
        {
            // Add a vertical force to the player.
            gamecontroller.grounded = false;

            // Change the force based on whether the player is on a ladder
            // The ladder reduces the jump height, so need to add more force
            m_Rigidbody2D.AddForce(new Vector2(0f, gamecontroller.climbing? m_JumpForce * 1.2f : m_JumpForce));
        }
    }


    // Chanye Jung
    // 
    // Change the direction the player is facing
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    // Cristian Rangel
    //
    // If player touches a deadzone, respawn at last checkpoint
    // If player touches a checkpoint, set that as the last checkpoint
    // If player touches an activatorPoint, save those as the last touched
    // If player touches a ladder, set player as climbing
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            transform.position = lastCP.transform.position;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HUD.GetComponent<UIKeys>().lostLife();
        } else if (collision.CompareTag("Checkpoint")) {
            lastCP = collision.gameObject;
        } else if (collision.CompareTag("Activator")) {
            var da = collision.gameObject.GetComponent<ActivatorPoint>();
            if (da.isDoorSwitch && da.activator)
                lastActivators[0] = da;
            else if (da.isDoorSwitch)
                lastActivators[1] = da;
        } else if (collision.CompareTag("Interactable")) {
            gamecontroller.climbing = true;
        } else if (collision.CompareTag("Key")) {
            hasKey = true;
        } else if (collision.CompareTag("Enemy")) {
            transform.position = lastCP.transform.position;
            lastActivators[1].activate(this);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HUD.GetComponent<UIKeys>().lostLife();
        }
    }

    // Cristian Rangel
    //
    // If player collides with an enemy, respawn
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.position = lastCP.transform.position;
            lastActivators[1].activate(this);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HUD.GetComponent<UIKeys>().lostLife();
        }
    }

    // Chanye Jung
    //
    // Function to check if the player would normally collide with a ceiling
    //
    // Checks if a standing player would collide with the ceiling
    void checkCeilingCollider ()
    {
        var ceilingCollider = Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround);
        // If the character has a ceiling preventing them from standing up, keep them crouching
        gamecontroller.hittingCeiling = gamecontroller.crouch =
            (ceilingCollider && !ceilingCollider.CompareTag("Checkpoint") &&
              !ceilingCollider.CompareTag("flag") && gamecontroller.grounded);
    }

    // Isaac Hintergardt, Cristian Rangel
    //
    // Function to determine if the player has hit a grapple point or enemy
    //   and act accordingly
    public void shootRope()
    {
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var angle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        var aimDirection = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, maxDist + 1, blanketMask);
        
        ropeRenderer.enabled = true;
        ropeRenderer.positionCount = 2;

        // If it hit a grapple or enemy
        if (hit.collider != null)
        {
            updateRopePositions(hit.point);
            // Player hit grapple
            if (hit.collider.CompareTag("grappleObject"))
            {
                gamecontroller.grappling = true;
                dj.enabled = true;
                dj.connectedAnchor = hit.point;
                dj.anchor = Vector2.zero;
                //dj.distance = maxDist;
                dj.distance = Vector2.Distance(hit.point, transform.position);
            }
            // Player hit chest
            else if (hit.collider.CompareTag("Interactable")) {
                var chest = hit.collider.gameObject;
                chest.GetComponent<ChestController>().wasHit();
            }
            // Player hit enemy
            else
            {
                var enemy = hit.collider.gameObject;
                var boss = enemy.GetComponent<BedBossController>();
                if (boss != null) {
                    if (boss.takeDamage(attackDamage) == 0) {
                        lastActivators[0].deactivate();
                        lastActivators[1].deactivate();
                    }
                } else {
                    enemy.SetActive(false);
                    createPickUp(enemy.transform.position);
                    lastActivators[0].deactivate();
                    lastActivators[1].deactivate();
                }
                enemy.SetActive(false);
                createPickUp(enemy.transform.position);

                //These were crashing the program I'm not sure what they do what commented out
                //the tutorial works fine -Bonita
                if (lastActivators[0] != null)
                    lastActivators[0].deactivate();
                if (lastActivators[1] != null)
                    lastActivators[1].deactivate();
            }
        }
        // If the player just clicked on the screen randomly
        else
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float rayDistance;
            if (new Plane(Vector3.forward, Vector3.zero).Raycast(ray, out rayDistance))
            {
                Vector3 pos = transform.position;
                Vector3 rayPoint = ray.GetPoint(rayDistance);
                rayPoint.z = 0;

                hit = Physics2D.Raycast(transform.position, aimDirection, maxDist, LayerMask.GetMask("Ground"));
                if (hit.collider != null)
                {
                    rayPoint = hit.point;
                }
                else
                {
                    // Change line length by the correct ratio to maintain the same slope
                    float numer = ((rayPoint - pos).normalized.y) / 2;
                    float denom = ((rayPoint - pos).normalized.x) / 2;

                    if (Vector3.Distance(rayPoint, pos) > maxDist)
                        // While the line is too long, reduce it
                        while (Vector3.Distance(rayPoint, pos) > maxDist) {
                            rayPoint.y -= numer;
                            rayPoint.x -= denom;
                        }
                    else if (Vector3.Distance(rayPoint, pos) < maxDist)
                        // While the line is too short, enlarge it
                        while (Vector3.Distance(rayPoint, pos) < maxDist) {
                            rayPoint.y += numer;
                            rayPoint.x += denom;
                        }
                }
                updateRopePositions(rayPoint);
            }
        }
    }

    // Isaac Hintergardt, Cristian Rangel
    //
    // Function to handle the rope cleanup
    public void releaseRope()
    {
        gamecontroller.letGo = true;
        ropeRenderer.enabled = false;
        dj.enabled = false;
    }

    // Isaac Hintergardt, Cristian Rangel
    //
    // Function to re-render the line to ensure it's between the player
    //   and grapple point at all times
    //
    // @param Vector3 position - Position of the second point of the rope,
    //   with the first being centered on the player
    void updateRopePositions(Vector3 position)
    {
        float xDist = position.x - transform.position.x;
        float yDist = position.y - transform.position.y;

        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, position);
    }

    // Bonita Galvan
    //
    // Function to generate a coin prefab in the place of an enemy death
    //
    // @param Vecetor3 enemyPos - The position of the enemy that died
    public void createPickUp(Vector3 enemyPos)
    {
        coin = Instantiate(coinPrefab);
        coin.transform.position = enemyPos;
    }
}
