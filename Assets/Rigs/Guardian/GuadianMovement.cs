using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuadianMovement : MonoBehaviour
{

    public float walkSpeed = 5;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");

        transform.position += transform.forward * v * Time.deltaTime * walkSpeed;

        animator.SetFloat("speed", Mathf.Abs(v * walkSpeed));

    }
}
