using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float inAirMultiplier;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform orientation;


    private bool grounded;
    private bool ready_to_jump;

    private float horizontal_input;
    private float vertical_input;

    private Vector3 move_direction;
    private Rigidbody rb;

    private bool movementLocked = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ready_to_jump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (!movementLocked) {
            MyInput();
        }
        
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        
    }


    private void FixedUpdate()
    {
        if (!movementLocked) {
            MovePlayer();  
        }
    }

    private void MyInput()
    {
        horizontal_input = Input.GetAxisRaw("Horizontal");
        vertical_input = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && ready_to_jump && grounded)
        {
            ready_to_jump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        move_direction = orientation.forward * vertical_input + orientation.right * horizontal_input;

        // on ground
        if(grounded)
            rb.AddForce(move_direction.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(move_direction.normalized * moveSpeed * 10f * inAirMultiplier, ForceMode.Force);
    }

    public void LockMovement() {
        movementLocked = true;
    }

    public void UnlockMovement() {
        movementLocked = false;
    }

    private void SpeedControl()
    {
        Vector3 flat_vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flat_vel.magnitude > moveSpeed)
        {
            Vector3 limited_vel = flat_vel.normalized * moveSpeed;
            rb.velocity = new Vector3(limited_vel.x, rb.velocity.y, limited_vel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        ready_to_jump = true;
    }
}