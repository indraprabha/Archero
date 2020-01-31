using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : CharacterControl
{
    public Material hit;
    public Material notHit;
    public Slider healthSlider;
    public float timeBetweenShots = 0.1f;
    float timer;

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

    private void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        SetSkinMaterial(notHit);

        // Attack only if enough time lapsed since last attack
        if (timer >= timeBetweenShots)
        {
            Attack();
        }
    }

    // FixedUpdate is called on every physics update 
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turn();
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
    void Turn ()
    {
        // Determine direction of rotation 
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - movement);

        // Smoothly rotate from current lookrotation to new one at given speed
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    void Animating(float h, float v)
    {
        // Set animator's IsWalking parameter if either 
        // horizontal or vertical axis input received
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

    public override void Attack()
    {
        // Reset timer
        timer = 0f;

        RaycastHit hit;
        EnemyControl[] enemies = FindObjectsOfType<EnemyControl>();
        foreach (EnemyControl enemy in enemies)
        {
            if (!enemy.isDead())
            {
                Debug.Log("Attacking enemy with health " + enemy.characterHealth+" hP");
                if (Physics.Raycast(transform.position, transform.TransformDirection(enemy.transform.position), out hit, Mathf.Infinity))
                {
                    enemy.FaceAttack();
                }
            }
        }
    }

    public void FaceAttack()
    {
        SetSkinMaterial(hit);
        characterHealth -= attackPower;

        // Set the health bar's value to the current health.
        healthSlider.value = characterHealth;
        Debug.Log("Player hit! health reduced to " + characterHealth + "hP ");

        if (characterHealth <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }
}
