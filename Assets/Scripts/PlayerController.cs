using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float groundSpeed;
    public float jumpSpeed;
    [Range(0f, 1f)]
    public float groundDecay;
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public bool grounded;
    public Animator animator;

    float xInput;
    float yInput; // Currently unused, but kept for future features

    void Update()
    {
        GetInput();
        HandleJump();

        //animation work WIP
        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(xInput));
    }

    void FixedUpdate()
    {
        CheckGround();
        MoveWithInput();
        ApplyFriction();
    }

    /*
     input will store a value between -1 and +1
     getAxisRaw() takes exactly -1 or +1
     GetAxis() Takes a value bewteen sand up to -1 to +1 (useful for acceleration)
     getting the axis is mapped to A/D, left/right arrow and joystick lef/right
     */
    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        // Jump should only trigger ONCE when you press W, and only if grounded
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }

    void MoveWithInput()
    {
        if (Mathf.Abs(xInput) > 0f)
        {
            // Note: if acceleration is intended to be per-second, multiply by Time.fixedDeltaTime.
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(body.linearVelocity.x + increment, -groundSpeed, groundSpeed);

            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y); /*Side note if you find any previous unity projects online and thery have "_______.velocity" 
                                                                                              it has been changed in this most recent unity update there are now linear and angular for 
                                                                                              the time being we only care for linear.*/

            //Turns the character based of where they are moving ie will 
            // look left if moving left will look right if moving right
            float direction = Mathf.Sign(xInput);
            transform.localScale = new Vector3(direction, 1, 1);
        }

    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
            //trigger the jup paramater which allows us to be able to detect when we are jumping
            if (animator != null)
                animator.SetBool("isJumping", true);

            // need some one to figure out how to get the jumping animation to work got idle and runing but jumping seems a to now want to work 
        }

        if (grounded)
        {
            if (animator != null)
                animator.SetBool("isJumping", false);
        }
    }

    //Ground collision function if the is collision box of "GroundCheck" on the player is touching a physics layer of the name "Ground". if it is true it returns true
    void CheckGround()
    {
        if (groundCheck != null)
            grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        else
            grounded = false;
    }

    void ApplyFriction()
    {
        if (grounded && Mathf.Approximately(xInput, 0f) && body.linearVelocity.y <= 0f)
        {
            body.linearVelocity = body.linearVelocity * groundDecay;
        }
    }
}
