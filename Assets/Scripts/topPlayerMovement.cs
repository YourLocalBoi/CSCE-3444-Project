using UnityEngine;

public class topPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;
    public bool canMove = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (animator != null)
            animator.updateMode = AnimatorUpdateMode.Normal; // ensure it stops with timeScale
    }

    // Update is called once per frame
    void Update()
    {
        // stop all actions when paused or movement is disabled
        if ((Time.timeScale == 0f) || (!canMove))
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (animator)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        //inputs go here
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }


    void FixedUpdate()
    {

        // stop movement when paused
        if ((Time.timeScale == 0f) || (!canMove))
            return;
        //movements happens here

        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
