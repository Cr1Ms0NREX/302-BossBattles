using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonMovement : MonoBehaviour
{
    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float walkSpreadX = .2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .6f;
    public float walkFootSpeed = 4;

    private CharacterController pawn;

    // Start is called before the first frame update
    void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v + transform.right * h;
        if (move.sqrMagnitude > 1) move.Normalize();

        pawn.SimpleMove(move * speed);

        AnimateWalk();
    }

    delegate void MoveFoot(float time, float x, FootRaycast foot);
    void AnimateWalk() {

        // Don't Repeat Yourself (DRY)

        MoveFoot moveFoot = (t, x, foot) =>
        {

            float y = Mathf.Cos(t) * walkSpreadY;
            float z = Mathf.Sin(t) * walkSpreadZ; // [-1, +1]

            if (y < 0) y = 0;

            y += .177f;

            foot.transform.localPosition = new Vector3(x, y, z);
        };
        float t = Time.time * walkFootSpeed;

        moveFoot.Invoke(t, -walkSpreadX, footLeft);
        moveFoot.Invoke(t + Mathf.PI, walkSpreadX, footRight);
    }
}
