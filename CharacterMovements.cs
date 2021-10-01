using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovements : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    public float jumpForce;
    public float fallGravity;
    public float jumpGravity;

    private RigidBody2D player;
}
    






    // Update is called once per frame
    void Update()
    {
        inputMovement = Input.GetAxis("Horizantal");

        if (Input.GetButtonDown("Jump") && Mathf.abs(rb.velocity.y) < 0.001f)
        {
            jumpQueued = true;
        }

        if (Input.GetButton("Jump") && rb.velocity.y > 0)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = fallGravity;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(inputMovement, 0) * movementSpeed);

        if (jumpQueued)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpQueued = false;
        }

    }
}
