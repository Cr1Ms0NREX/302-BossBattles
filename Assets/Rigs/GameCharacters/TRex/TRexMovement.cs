using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRexMovement : MonoBehaviour
{

    enum Mode
    {
        Idle,
        Walk,
        Jump,
        Attack
    }

    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float footSeparateAmount = .2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .6f;
    public float walkFootSpeed = 4;

    private CharacterController pawn;

    /// <summary>
    /// The current vertical velocity in meters/second
    /// </summary>
    public float velocityY = 0;

    public float gravity = 50;
    public float jumpImpulse = 50;

    private Mode mode = Mode.Idle;

    private Vector3 input;

    private float walkTime = 0;

    private Camera cam;

    private Quaternion targetRotation;

    // Animator
    Animator animator;
    int isWalkingHash;
    int isWalkingHash1;
    int isWalkingHash2;
    int isWalkingHash3;
    int isJumpingHash;

    // Start is called before the first frame update
    void Start()
    {
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;

        // Animator
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingHash1 = Animator.StringToHash("isWalking1");
        isWalkingHash2 = Animator.StringToHash("isWalking2");
        isWalkingHash3 = Animator.StringToHash("isWalking3");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        // Animator
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isWalking1 = animator.GetBool(isWalkingHash1);
        bool isWalking2 = animator.GetBool(isWalkingHash2);
        bool isWalking3 = animator.GetBool(isWalkingHash3);
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool downPressed = Input.GetKey("s");
        bool rightPressed = Input.GetKey("d");
        bool isJumping = animator.GetBool(isJumpingHash);
        bool jumpPressed = Input.GetKey("space");
        // If player presses w key
        if (!isWalking && forwardPressed)
        {
            // Then set the isWalking boolean to be true
            animator.SetBool(isWalkingHash, true);
        }

        // If player is not pressing w key
        if (isWalking && !forwardPressed)
        { 
            // Then set the isWalking boolean to be false
            animator.SetBool(isWalkingHash, false);
        }

        if (!isWalking1 && leftPressed)
        {
            // Then set the isWalking boolean to be true
            animator.SetBool(isWalkingHash1, true);
        }

        if (isWalking1 && !leftPressed)
        {
            // Then set the isWalking boolean to be false
            animator.SetBool(isWalkingHash1, false);
        }

        if (!isWalking2 && downPressed)
        {
            // Then set the isWalking boolean to be true
            animator.SetBool(isWalkingHash2, true);
        }

        if (isWalking2 && !downPressed)
        {
            // Then set the isWalking boolean to be false
            animator.SetBool(isWalkingHash2, false);
        }

        if (!isWalking3 && rightPressed)
        {
            // Then set the isWalking boolean to be true
            animator.SetBool(isWalkingHash3, true);
        }

        if (isWalking3 && !rightPressed)
        {
            // Then set the isWalking boolean to be false
            animator.SetBool(isWalkingHash3, false);
        }

        // If player presses the left mouse button
        if (!isJumping && jumpPressed)
        {
            // Then set the is walking boolean to be treu
            animator.SetBool(isJumpingHash, true);
        }

        // If player is not pressing the left mouse button
        if (isJumping && !jumpPressed)
        {
            // Then set the is walking boolean to be treu
            animator.SetBool(isJumpingHash, false);
        }

        // End Animator
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        input = camForward * v + camRight * h;
        if (input.sqrMagnitude > 1) input.Normalize();

        

        // Set movement mode based on movement input
        float threshold = .1f;
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle; // Terniary Opporator
        if(mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up);

        if(pawn.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;

        pawn.Move((input * speed + Vector3.down * velocityY)* Time.deltaTime);

        if (pawn.isGrounded)
        {
            velocityY = 0;
        }
        else
        {
            mode = Mode.Jump;
        }

        Animate();
    }

    void Animate()
    {

        transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);

        switch (mode)
        {
            case Mode.Idle:
                AnimateIdle();
                break;
            case Mode.Walk:
                AnimateWalk();
                break;
            case Mode.Jump:
                AnimateJump();
                break;
        }
    }

    void AnimateJump()
    {
        // TODO:

        // Lift Legs?
        // Lift Hands/Arms?
        // Adjust Spikes/Hair?
        // Use Vertical Velocity
    }

    void AnimateIdle(){
        footLeft.SetPositionHome();
        footRight.SetPositionHome();
    }

    delegate void MoveFoot(float time, FootRaycast foot);
    void AnimateWalk() {

        // Don't Repeat Yourself (DRY)

        /*MoveFoot moveFoot = (t, foot) =>
        {

            float y = Mathf.Cos(t) * walkSpreadY; // Vertical Movement
            float lateral = Mathf.Sin(t) * walkSpreadZ; // Lateral Movement // [-1, +1]

            Vector3 localDir = foot.transform.parent.InverseTransformDirection(input);
            Vector3 separarteDir = Vector3.Cross(Vector3.up, localDir);

            float z = lateral * localDir.z;
            float x = lateral * localDir.x;

            float alignment = Mathf.Abs(Vector3.Dot(localDir, Vector3.forward));
            // 1 = forward
            // -1 = backwards
            // 0 = strafing

            if (y < 0) y = 0;

            //y += .177f; // an offset to the starting position of the feet. (Verticaly)

            foot.SetPositionOffset(new Vector3(x, y, z), footSeparateAmount * alignment);
        };

        walkTime += Time.deltaTime * input.sqrMagnitude * walkFootSpeed;

        moveFoot.Invoke(walkTime, footLeft);
        moveFoot.Invoke(walkTime + Mathf.PI, footRight);*/
    }
}
