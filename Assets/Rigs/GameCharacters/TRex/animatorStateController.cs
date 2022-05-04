using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorStateController : MonoBehaviour
{ 
    Animator animator;
    int isWalkingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
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
    }
}
