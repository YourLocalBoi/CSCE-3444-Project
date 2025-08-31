using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public float groundSpeed;
    public float jumpSpeed;
    [Range(0f, 1f)]
    public float groundDecay;
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;
    public bool grounded;
    float xInput;
    float yInput;


    void Update()
    {
        GetInput();
        HandleJump();
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
        if (Mathf.Abs(xInput) > 0)
        {
            body.linearVelocity = new Vector2(xInput * groundSpeed, body.linearVelocity.y); /*Side note if you find any previous unity projects online and thery have "_______.velocity" 
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
        if (Mathf.Abs(yInput) > 0 && grounded) 
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, yInput * jumpSpeed);
        }  
    }




    //Ground collision function if the is collision box of "GroundCheck" on the player is touching a physics layer of the name "Ground". if it is true it returns true
    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction()
    {
       if (grounded && xInput == 0 && yInput == 0)
        {
            body.linearVelocity *= groundDecay;
        }
    }
}   
