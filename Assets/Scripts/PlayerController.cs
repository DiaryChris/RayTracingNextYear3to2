using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed;
    public float maxJumpForce;

    private Rigidbody rb;
    private SphereCollider sphereCollider;

    private bool isGrounded;
    private LayerMask whatIsGround;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();

        whatIsGround = 1 << LayerMask.NameToLayer("Ground");
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        Move(x, jump);

    }

    private void FixedUpdate()
    {
        CheckGrounded();
        //Debug.Log(isGrounded);
    }

    private void Move(float x, bool jump)
    {
        if (Mathf.Abs(x) > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        rb.velocity = new Vector2(x * maxSpeed, rb.velocity.y);

        if (jump && isGrounded)
        {
            rb.AddForce(Vector2.up * maxJumpForce);
        }
        //Debug.Log(jump);
    }

    private void CheckGrounded()
    {
        Vector2 groundCheck = new Vector2(sphereCollider.bounds.center.x, sphereCollider.bounds.min.y);
        Vector2 checkSize = new Vector2(transform.localScale.x * sphereCollider.radius * 0.49f, 0.05f);
        //Debug.Log(groundCheck);
        isGrounded = Physics2D.OverlapArea(groundCheck - checkSize, groundCheck + checkSize, whatIsGround);
    }
}