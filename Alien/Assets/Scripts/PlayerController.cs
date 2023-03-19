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

    [SerializeField] private float interactRadius; 
    [SerializeField] private LayerMask interactLayer; 

    [SerializeField] private CapsuleCollider interactCollider;

    [SerializeField] private InteractManager interactManager;


    private bool grounded;
    private bool ready_to_jump;

    private float horizontal_input;
    private float vertical_input;

    private Vector3 move_direction;
    private Rigidbody rb;

    private RaycastHit[] interactHits;
    

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

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        

        for (int i = 0; i < interactHits.Length; i++)
        {
            //Debug.Log("HIT");
            RaycastHit hit = interactHits[i];
            interactManager.PromptInteract(hit.transform.GetComponent<Interactable>());

        }
    }

    private void checkInteracts() {

        Vector3 realCenter = interactCollider.transform.position + interactCollider.center;
        Vector3 halfVector = (interactCollider.height * 0.5f - interactCollider.radius) * (realCenter - transform.position).normalized;
        Vector3 p1 = realCenter - halfVector;
        Vector3 p2 = realCenter + halfVector;

        interactHits = Physics.CapsuleCastAll(p1, p2, interactCollider.radius, p2 - p1, interactCollider.height, interactLayer);
        //Debug.DrawRay(p1 - realCenter.normalized * interactCollider.radius, (p2 - p1), Color.green);

    }

    private void FixedUpdate()
    {
        MovePlayer();
        checkInteracts();

        
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
