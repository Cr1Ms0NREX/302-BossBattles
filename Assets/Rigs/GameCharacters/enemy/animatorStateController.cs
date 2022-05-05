using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class animatorStateController : MonoBehaviour
{
    //CharacterController pawn;
    NavMeshAgent agent;
    Transform navTarget;
    private Vector3 navTargetDir;
    public float speed = 2;
    public float velocityY = 0;
    [Range(-10, -1)]
    public float gravity = -1;
    public float jumpImpulse = 50;
    private float velocityVertical = 0;
    private Vector3 input;

    private Quaternion targetRotation;

    Animator animator;
    int isWalkingHash;
    //int isDeadHash;
    int isAttackingHash;


    // Start is called before the first frame update
    void Start()
    {
        //pawn = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5;

        PlayerTargeting player = FindObjectOfType<PlayerTargeting>();
        //navTarget = player.transform;

        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        //isDeadHash = Animator.StringToHash("isDead");
        isAttackingHash = Animator.StringToHash("isAttacking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool playerInSight = Input.GetKey("w");
        //bool isDead = animator.GetBool(isDeadHash);
        bool isAttacking = animator.GetBool(isAttackingHash);
        // If player presses w key
        if (!isWalking && playerInSight)
        {
            // Then set the isWalking boolean to be true
            animator.SetBool(isWalkingHash, true);
        }

        // If player is not pressing w key
        if (isWalking && !playerInSight)
        {
            // Then set the isWalking boolean to be false
            animator.SetBool(isWalkingHash, false);
        }
        // Set movement mode based on movement input
        float threshold = .1f;
        
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        navTargetDir = transform.forward * v + transform.right * h;
        if (navTargetDir.sqrMagnitude > 1) navTargetDir.Normalize();

        if (navTarget) agent.destination = navTarget.transform.position;
        Vector3 moveAmount = navTargetDir * speed * velocityVertical;
        agent.Move(moveAmount * Time.deltaTime);
        if (agent)
        {
            //cooldownJumpWindow = .5f;
            velocityVertical = 0;
            Vector3 navTargetDirLocal = transform.InverseTransformDirection(navTargetDir);
            Vector3 axis = Vector3.Cross(Vector3.up, navTargetDir);
            float alignment = Vector3.Dot(navTargetDirLocal, Vector3.forward);
            alignment = Mathf.Abs(alignment);

            float degrees = AnimMath.Lerp(10, 40, alignment);
            float speed = 10;
            float wave = Mathf.Sin(Time.time * speed) * degrees;
        }
        velocityY += gravity * Time.deltaTime;
    }
}
