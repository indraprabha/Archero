using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : CharacterControl
{
    Vector3 movement;
    Animator anim;
    Rigidbody characterRigidbody;

    // Awake is called at beginning irrespective 
    // of script enabled or not
    private void Awake()
    {
        anim = GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called on every physics update 
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turn(h, v);
        Animating(h, v);
    }

    // Move Character as per user input
    void Move(float h, float v)
    {
        // Set X and Z axes according to horizontal and vertical inputs 
        movement.Set(h, 0f, v);

        // Normalize movement in both axes with desired speed
        movement = movement.normalized * speed * Time.deltaTime;
        characterRigidbody.MovePosition(transform.position + movement);
    }

    // Turn Character towards the direction of user input
    void Turn (float h, float v)
    {
        Vector3 lookVector = new Vector3(h, 0f, v);
        Quaternion targetRotation = Quaternion.LookRotation(lookVector);

        // smooth rotate from current lookrotation to new one at given speed
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    void Animating(float h, float v)
    {
        // Set animator's IsWalking parameter if either 
        // horizontal or vertical axis input received
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

}
