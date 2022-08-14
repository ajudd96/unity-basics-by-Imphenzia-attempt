using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // SerializeField will expose field to Inspector
    // groundCheckTransform (located at bottom of player)
    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask playerMask;

    private Rigidbody rigidbodyComponent;

    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private int superJumpsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        // Retrieves current horizontal player movement
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called once every physics update 
    private void FixedUpdate()
    {

        // Adjusts player position according to horizonatal key presses while maintaining y velocity
        rigidbodyComponent.velocity = new Vector3(horizontalInput, rigidbodyComponent.velocity.y, 0);

        // Is checking if in air (not colliding with anything)
        // playerMask excludes actual player from collision check
        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        // Adds y force when space button is pressed
        if (jumpKeyWasPressed)
        {
            float jumpPower = 5f;
            if (superJumpsRemaining > 0)
            {
                jumpPower *= 2;
                superJumpsRemaining--;
            }

            rigidbodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
            superJumpsRemaining++;
        }
    }

}
