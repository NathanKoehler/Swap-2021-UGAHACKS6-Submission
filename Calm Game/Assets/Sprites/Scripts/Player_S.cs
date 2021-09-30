using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player_S : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Cinemachine.CinemachineVirtualCamera cinemachine;
    public float jumpHeight;
    public float jumpingGravity;
    public float normalGravity;
    public float locationInherit;
    public float locationPrevious;
    public bool isCrouch;
    public Vector3 playerStartPoint;
    public GameObject projectile;
    public String horizontalAxis;
    public String verticalAxis;
    public String jumpInput;
    public String playerName;
    public Light2D playerLight;
    public Transform startLocation;

    [HideInInspector]
    public bool stunned;
    [HideInInspector]
    public bool facingDir = true;

    private static bool waitIsRunning;
    private IEnumerator Jumping;
    private ArrayList collider2Ds;
    private Collider2D colliderObj;
    private Vector2 rawInputs;
    private Vector2 playerCheckpoint;
    private Animator anim;
    private Transform parentObj;
    private Transform childObj;
    private GameObject player;
    private SpriteRenderer rend;
    private Transform PlayerTransform;
    private Transform TeleportGoal;
    private Color playerColor;
    private bool disableSlow;
    
    private bool slowDown;
    private bool cancelX;
    private bool lockInputs;
    private bool resetVel;
    private bool slippery;
    [SerializeField]
    private bool canJump = false;
    private bool isJumpPress = false;
    private bool playerEnabled = true;
    public bool isDead;
    public bool isRed;
    public SpriteRenderer sr;
    public Sprite[] sprites;


    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        MasterController_S.players.Add(this);
        collider2Ds = new ArrayList();
        player = GameObject.Find(playerName);
        rend = GetComponentInChildren<SpriteRenderer>();
        Debug.Log(rend);
        playerColor = rend.color;
        anim = GetComponentInChildren<Animator>();
        rend.enabled = true;
        playerStartPoint = player.transform.position;
        playerCheckpoint = playerStartPoint;
        
        rigid.gravityScale = normalGravity;
        childObj = rend.transform;
    }


    // Update is called once per frame
    void Update()
    {
        SpriteChange();
        ModifyVelocity();
        WhereToMove();
        Switch();
        Dead();
        //Debug.Log(rigid.velocity.x);

        if (canJump)
        {
            if (!(Mathf.Abs(rawInputs.x) > 0))
            {
                //anim.SetInteger("Status", 0);
            }
            else
            {
                //anim.SetInteger("Status", 1);
            }
        }
        else if (rigid.velocity.y > 0)
        {
            //anim.SetInteger("Status", 2);
        }
        else if (rigid.velocity.y <= 0)
        {
            //anim.SetInteger("Status", 3);
        }
    }


    private void FixedUpdate()
    {
        Move();
    }
    

    private void ModifyVelocity()
    {
        
        float XVelocity = rigid.velocity.x;
        float YVelocity = rigid.velocity.y;
        //Debug.Log("Vel: " + YVelocity);
        //Debug.Log("For:" + rawInputs.y);
        if (!facingDir && rawInputs.x > 0)
        {
            facingDir = true; // Determines the player facing right
            //rend.flipX = false;
            childObj.localPosition = new Vector2(-childObj.localPosition.x, childObj.localPosition.y);
        }
        else if (facingDir && rawInputs.x < 0)
        {
            facingDir = false; // Determines the player is facing left
            //rend.flipX = true;
            childObj.localPosition = new Vector2(-childObj.localPosition.x, childObj.localPosition.y);
        }

        if (isJumpPress)
        {
            if (rawInputs.y > 0 && YVelocity > 0) // Is the player choosing to continue to jump?
            {
                rigid.gravityScale = jumpingGravity; // Adds a lighter jump when you hold jump
            }
            else
            {
                rigid.gravityScale = normalGravity;
                isJumpPress = false;
            }
        }

        if (!resetVel)
        {
            if (cancelX && !disableSlow)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
            if (slowDown && !slippery && !disableSlow)
            {
                rigid.velocity = new Vector2(rigid.velocity.x * 6f / 10f, rigid.velocity.y);
            }

            // Structured to isolate the velocity and force it to pass certain parameters
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -10, 10), Mathf.Clamp(rigid.velocity.y, -100, 100)); // Prevents Crazy Fast Movement
        }
        else
        {
            resetVel = false;
        }

        if (canJump)
        {
            if (rawInputs.y > 0 && playerEnabled && Jumping == null) // Is the player choosing to jump up?
            {
                Debug.Log("Jump");
                isJumpPress = true;
                if (rigid.velocity.y < 0)
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(new Vector2(0, rawInputs.y * jumpHeight), ForceMode2D.Impulse);
                Jumping = StartedJumping();
                StartCoroutine(Jumping);
            }
        }

        if (parentObj != null)
        {
            rigid.position = new Vector2(rigid.position.x + parentObj.position.x - locationInherit - locationPrevious, rigid.position.y);
            locationPrevious = parentObj.position.x - locationInherit;
        }
    }


    private void WhereToMove()
    {
        if (playerEnabled)
        {
            rawInputs = new Vector2(Input.GetAxisRaw(horizontalAxis), Input.GetAxisRaw(verticalAxis));
            if (Input.GetButton(jumpInput) && !isDead)
            {
                rawInputs.y = 1;
            }

            if (rawInputs.y < 0)
            {
                isCrouch = true;
            }
            else
            {
                isCrouch = false;
            }
        }
        else
        {
            rawInputs = Vector2.zero;
        }
        if ((!facingDir && Input.GetAxisRaw(horizontalAxis) > 0) || (facingDir && Input.GetAxisRaw(horizontalAxis) < 0)) // When to reset Horizontal Movement
        {
            cancelX = true;
        }
        else
        {
            cancelX = false;
            if (Input.GetAxisRaw(horizontalAxis) == 0)
            {
                slowDown = true;
            }
            else
            {
                slowDown = false;
            }
        }
    }

    private void Move()
    {
        Vector2 modifiedInputs = Vector2.zero;
        if (!isDead)
        {
            modifiedInputs = new Vector2(rawInputs.x * 120, rawInputs.y * 0); // Makes the Jump much heavier
        }
        if (!lockInputs)
        {
            rigid.AddForce(modifiedInputs);
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {                   

    }

    public void SetCanJump(bool newCanJump)
    {
        canJump = newCanJump;
    }

    public void PositionInheritance(bool activate, Transform other) // Only works if never used by multiple "velocity" parents at once
    {
        if (activate)
        {
            transform.SetParent(other);
            parentObj = other;
            locationPrevious = 0;
            locationInherit = other.position.x;
        }
        else
        {
            transform.SetParent(null);
            parentObj = null;
        }
    }


    public void ReEnableCollision()
    {
        foreach (Collider2D collider in collider2Ds)
        {
            Debug.Log("Ignored");
            Physics2D.IgnoreCollision(colliderObj, collider, false);
        }
        collider2Ds.Clear();
    }
    public void Switch()
    {
        if (Input.GetKeyUp("space"))
        {
            if (playerName.Equals("Player Obj (1)"))
            {
                MasterController_S.changeColor();
                //SwitchCode();
            }
            if (playerName.Equals("Player Obj"))
            {
                //SwitchCode();
            }
            isRed = !isRed;
        }
    }
    private void SpriteChange()
    {
        if (isRed)
        {
            sr.sprite = sprites[0];
        }
        else
        {
            sr.sprite = sprites[1];
        }
    }
    public void SwitchControls()
    {
        if (isRed)
        {
            horizontalAxis = "Horizontal 2";
            verticalAxis = "Vertical 2";
            jumpInput = "Jump 2";
        }
        else
        {
            horizontalAxis = "Horizontal";
            verticalAxis = "Vertical";
            jumpInput = "Jump 1";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("RedDoor") && isRed == true)
        {
            MasterController_S.redReady = true;
        }

        if (collision.name.Equals("BlueDoor") && isRed == false)
        {
            MasterController_S.blueReady = true;
        }

        if (collision.name.Equals("RedSpikes") && isRed == false)
        {
            isDead = true;
        }
        if (collision.name.Equals("BlueSpikes") && isRed == true)
        {
            isDead = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("RedDoor") && isRed == true)
        {
            MasterController_S.redReady = false;
        }

        if (collision.name.Equals("BlueDoor") && isRed == false)
        {
            MasterController_S.blueReady = false;
        }
    }
    IEnumerator Wait()
    {
        rend.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        rend.gameObject.SetActive(true);
        waitIsRunning = false;
        MasterController_S.resetPosition();
    }

    public void Dead()
    {
        if (isDead && !waitIsRunning)
        {
            waitIsRunning = true;
            isDead = false;
            StartCoroutine(Wait());
        }

    }

    public void tpBack()
    {
        StopCoroutine("Wait");
        isDead = false;
        transform.position = startLocation.position;
    }


    public void StopCollision(Collider2D collider, Collider2D other)
    {
        //Debug.Log("ignored");
        if (stunned)
        {
            Physics2D.IgnoreCollision(collider, other, true);

            colliderObj = collider;

            collider2Ds.Add(other);
        }
    }

    IEnumerator StartedJumping()
    {
        yield return new WaitForSeconds(0.2f);
        Jumping = null;
    }
}
